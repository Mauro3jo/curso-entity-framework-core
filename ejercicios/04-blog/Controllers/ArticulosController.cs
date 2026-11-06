using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

public class ArticulosController(BlogDbContext contexto) : Controller
{
    public async Task<IActionResult> Index(string? buscar, int pagina = 1)
    {
        pagina = Math.Clamp(pagina, 1, 1000);
        var consulta = contexto.Articulos.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(buscar)) consulta = consulta.Where(a => a.Titulo.Contains(buscar));
        var filas = await consulta.OrderBy(a => a.Id).Skip((pagina - 1) * 10).Take(10)
            .Select(a => new ArticuloFila(a.Id, a.Titulo, a.Categoria.Nombre)).ToListAsync();
        return View(new ListadoArticulos(filas, buscar, pagina));
    }

    public async Task<IActionResult> Detalles(int id)
    {
        var datos = await contexto.Articulos.AsNoTracking().Where(a => a.Id == id)
            .Select(a => new { a.Id, a.Titulo, a.Contenido, Categoria = a.Categoria.Nombre, Autor = a.Autor.Nombre,
                Etiquetas = a.Etiquetas.Select(e => e.Nombre).ToList(), Comentarios = a.Comentarios.Select(c => c.Texto).ToList(), a.Version })
            .AsSplitQuery().SingleOrDefaultAsync();
        if (datos is null) return NotFound();
        return View(new DetalleArticulo(datos.Id, datos.Titulo, datos.Contenido, datos.Categoria, datos.Autor,
            datos.Etiquetas, datos.Comentarios, Convert.ToBase64String(datos.Version)));
    }

    public async Task<IActionResult> Crear()
    {
        await CargarOpcionesAsync();
        return View("Formulario", new FormularioArticulo());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(FormularioArticulo formulario)
    {
        await ValidarReferenciasAsync(formulario);
        if (!ModelState.IsValid) { await CargarOpcionesAsync(); return View("Formulario", formulario); }
        var articulo = new Articulo();
        CopiarCampos(formulario, articulo);
        contexto.Articulos.Add(articulo);
        await contexto.SaveChangesAsync();
        return RedirectToAction(nameof(Detalles), new { id = articulo.Id });
    }

    public async Task<IActionResult> Editar(int id)
    {
        var articulo = await contexto.Articulos.AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);
        if (articulo is null) return NotFound();
        await CargarOpcionesAsync();
        return View("Formulario", new FormularioArticulo { Titulo = articulo.Titulo, Contenido = articulo.Contenido,
            TiempoLectura = articulo.TiempoLectura, CategoriaId = articulo.CategoriaId, AutorId = articulo.AutorId,
            Version = Convert.ToBase64String(articulo.Version) });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, FormularioArticulo formulario)
    {
        var articulo = await contexto.Articulos.SingleOrDefaultAsync(a => a.Id == id);
        if (articulo is null) return NotFound();
        await ValidarReferenciasAsync(formulario);
        var version = LeerVersion(formulario.Version);
        if (version is null) ModelState.AddModelError(nameof(formulario.Version), "Version invalida.");
        if (!ModelState.IsValid) { await CargarOpcionesAsync(); return View("Formulario", formulario); }
        contexto.Entry(articulo).Property(a => a.Version).OriginalValue = version!;
        CopiarCampos(formulario, articulo);
        try { await contexto.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { return Conflict("El articulo cambio o fue eliminado; vuelve a cargarlo antes de guardar."); }
        return RedirectToAction(nameof(Detalles), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrar(int id, string? version)
    {
        var articulo = await contexto.Articulos.SingleOrDefaultAsync(a => a.Id == id);
        if (articulo is null) return NotFound();
        var original = LeerVersion(version);
        if (original is null) return BadRequest("Version invalida.");
        contexto.Entry(articulo).Property(a => a.Version).OriginalValue = original;
        contexto.Articulos.Remove(articulo);
        try { await contexto.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { return Conflict("El articulo cambio; revisalo antes de borrarlo."); }
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarReferenciasAsync(FormularioArticulo formulario)
    {
        if (!await contexto.Categorias.AnyAsync(c => c.Id == formulario.CategoriaId)) ModelState.AddModelError(nameof(formulario.CategoriaId), "Categoria inexistente.");
        if (!await contexto.Usuarios.AnyAsync(u => u.Id == formulario.AutorId)) ModelState.AddModelError(nameof(formulario.AutorId), "Autor inexistente.");
    }

    private async Task CargarOpcionesAsync()
    {
        ViewBag.Categorias = await contexto.Categorias.AsNoTracking().OrderBy(c => c.Nombre).Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToListAsync();
        ViewBag.Autores = await contexto.Usuarios.AsNoTracking().OrderBy(u => u.Nombre).Select(u => new SelectListItem(u.Nombre, u.Id.ToString())).ToListAsync();
    }

    private static void CopiarCampos(FormularioArticulo formulario, Articulo articulo)
    {
        articulo.Titulo = formulario.Titulo; articulo.Contenido = formulario.Contenido;
        articulo.TiempoLectura = formulario.TiempoLectura; articulo.CategoriaId = formulario.CategoriaId; articulo.AutorId = formulario.AutorId;
    }

    private static byte[]? LeerVersion(string? texto)
    {
        try { var valor = Convert.FromBase64String(texto ?? ""); return valor.Length == 8 ? valor : null; }
        catch (FormatException) { return null; }
    }
}
