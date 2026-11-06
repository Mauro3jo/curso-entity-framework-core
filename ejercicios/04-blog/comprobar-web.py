"""Comprobación HTTP con datos ficticios; requiere el blog en 127.0.0.1:5087."""
from html import unescape
from html.parser import HTMLParser
from http.cookiejar import CookieJar
from urllib.error import HTTPError
from urllib.parse import urlencode
from urllib.request import build_opener, HTTPCookieProcessor, Request
from uuid import uuid4
import re

class Formulario(HTMLParser):
    def __init__(self, html):
        super().__init__()
        self.campos = {}
        self.selector = None
        self.feed(html)

    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        if tag == "input" and "name" in attrs:
            self.campos[attrs["name"]] = attrs.get("value", "")
        if tag == "select":
            self.selector = attrs.get("name")
        if tag == "option" and self.selector:
            if self.selector not in self.campos or "selected" in attrs:
                self.campos[self.selector] = attrs.get("value", "")

    def handle_endtag(self, tag):
        if tag == "select": self.selector = None

cliente = build_opener(HTTPCookieProcessor(CookieJar()))
base = "http://127.0.0.1:5087"

def pedir(ruta, datos=None):
    request = Request(base + ruta, data=None if datos is None else urlencode(datos).encode())
    try:
        with cliente.open(request, timeout=15) as response:
            return response.status, response.read().decode(), response.url
    except HTTPError as error:
        return error.code, error.read().decode(), error.url

identificador = uuid4().hex[:10]
assert pedir("/Articulos")[0] == 200
status, html, _ = pedir("/Articulos/Crear")
assert status == 200
campos = Formulario(html).campos
assert campos.get("__RequestVerificationToken")
campos.update(Titulo="<script>alert(1)</script>" + identificador, Contenido="Contenido ficticio", TiempoLectura="5", Id="999999")
sin_token = {k: v for k, v in campos.items() if k != "__RequestVerificationToken"}
assert pedir("/Articulos/Crear", sin_token)[0] == 400
assert pedir("/Articulos/Crear", dict(campos, Titulo=""))[0] == 200
assert pedir("/Articulos/Crear", dict(campos, CategoriaId="2147483647"))[0] == 200
status, html, url = pedir("/Articulos/Crear", campos)
assert status == 200 and "/Articulos/Detalles/" in url, (status, url, re.findall(r"<li>(.*?)</li>", html))
articulo_id = int(url.rsplit("/", 1)[1])
assert articulo_id != 999999
assert "&lt;script&gt;" in html and "<script>alert(1)</script>" not in html
status, listado, _ = pedir("/Articulos?" + urlencode({"buscar": identificador, "pagina": 0}))
assert status == 200 and identificador in listado and "Relaciones con EF Core" not in listado
_, editar, _ = pedir(f"/Articulos/Editar/{articulo_id}")
modificacion = Formulario(editar).campos
modificacion.update(Titulo="Actualizado " + identificador, Contenido="Contenido editado", TiempoLectura="10")
assert pedir(f"/Articulos/Editar/{articulo_id}", modificacion)[0] == 200
assert pedir(f"/Articulos/Editar/{articulo_id}", dict(modificacion, Titulo="Obsoleto"))[0] == 409
assert pedir(f"/Articulos/Editar/{articulo_id}", dict(modificacion, Version="invalida"))[0] == 200
_, detalle, _ = pedir(f"/Articulos/Detalles/{articulo_id}")
assert "Actualizado " + identificador in unescape(detalle) and "Obsoleto" not in detalle
borrado = Formulario(detalle).campos
assert pedir(f"/Articulos/Borrar/{articulo_id}")[0] == 405
assert pedir(f"/Articulos/Borrar/{articulo_id}", dict(borrado, version=modificacion["Version"]))[0] == 409
assert pedir(f"/Articulos/Borrar/{articulo_id}", borrado)[0] == 200
assert pedir(f"/Articulos/Detalles/{articulo_id}")[0] == 404
assert pedir("/Articulos/Detalles/2147483647")[0] == 404
print("CRUD HTTP, filtros, validacion, antifalsificacion, encoding HTML, DTO y concurrencia: correctos.")
