# 04. Blog: relaciones entre entidades

Las [consultas LINQ y operaciones masivas](consultas.md) tienen una comprobación separada con `--consultas`.

Reconstrucción basada en los nombres de entidades y temas de `RelacionesConEFCore.rar`: usuarios, perfiles, categorías, artículos, comentarios y etiquetas. El código y los datos de demostración se escribieron de nuevo.

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --verificar
```

Requiere la instancia `(localdb)\CodexEfCoreCurso` preparada en Database First. La comprobación crea una base con nombre aleatorio `EfCoreCursoChecks_...`, verifica relaciones 1:1, 1:N y N:N, borrado restringido y cascadas, y elimina exclusivamente esa base temporal al terminar.

La categoría con artículos no se puede borrar; borrar un artículo elimina sus comentarios y enlaces, pero conserva las etiquetas. Borrar un usuario sin artículos ni comentarios elimina su perfil. Las relaciones se configuran explícitamente para evitar rutas de cascada ambiguas.

Para preparar una base persistente de datos ficticios:

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --crear-demo
```

Crea `EfCoreCursoBlog04` únicamente si no existe. No reemplaza una base anterior. El arranque web normal no crea ni modifica el esquema.

Este bloque usa `EnsureCreated` para una base de ejemplo. Las migraciones están en el primer ejercicio; no se mezclan ambos métodos en esta base.

## CRUD web de artículos

Después de crear la base de ejemplo:

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj --urls http://127.0.0.1:5087
```

Abrir `http://127.0.0.1:5087/Articulos`. Hay listado con búsqueda y paginación, detalle, creación, edición y borrado de artículos. Las vistas Razor usan formularios con token antifalsificación. El controlador recibe un formulario con campos permitidos y comprueba que existan el autor y la categoría.

La columna `rowversion` detecta ediciones y borrados con una versión desactualizada; esos casos devuelven HTTP 409. Un formulario de creación no requiere versión porque el registro todavía no existe.

Con el servidor encendido, ejecutar las pruebas HTTP (requieren Python 3):

```powershell
python ejercicios/04-blog/comprobar-web.py
```

Las pruebas crean y borran su propio artículo ficticio. Revisan el CRUD, filtros, campos inválidos, referencias inexistentes, token antifalsificación, codificación HTML, campos no permitidos y concurrencia. También comprueban respuestas 404 y que el borrado no acepte GET.

El proyecto es una práctica local sin inicio de sesión. No debe exponerse como un sitio de producción: cualquier visitante del ejemplo puede gestionar sus artículos. En esta reconstrucción se implementó CRUD de artículos; los demás modelos se usan para practicar relaciones y consultas.
