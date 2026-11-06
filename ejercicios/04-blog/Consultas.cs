using Microsoft.EntityFrameworkCore;

namespace Blog;

public static class Consultas
{
    public static async Task VerificarAsync()
    {
        var nombre = "EfCoreCursoConsultas_" + Guid.NewGuid().ToString("N");
        var opciones = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlServer($"Server=(localdb)\\CodexEfCoreCurso;Database={nombre};Integrated Security=True;Encrypt=True;TrustServerCertificate=True").Options;
        await using var contexto = new BlogDbContext(opciones);
        Comprobaciones.Exigir(await contexto.Database.EnsureCreatedAsync(), "Se requiere una base temporal nueva.");
        try
        {
            await DatosDemo.InsertarAsync(contexto);
            var categoriaId = await contexto.Categorias.Select(c => c.Id).SingleAsync();
            var autorId = await contexto.Usuarios.Select(u => u.Id).SingleAsync();
            contexto.Articulos.AddRange(
                new Articulo { Titulo = "Borrador", Contenido = "Ejemplo", TiempoLectura = 20, CategoriaId = categoriaId, AutorId = autorId },
                new Articulo { Titulo = "Publicado", Contenido = "Ejemplo", TiempoLectura = 15, CategoriaId = categoriaId, AutorId = autorId, Publicado = true });
            await contexto.SaveChangesAsync();
            contexto.ChangeTracker.Clear();

            IQueryable<Articulo> publicados = contexto.Articulos.AsNoTracking().Where(a => a.Publicado);
            Comprobaciones.Exigir(publicados.ToQueryString().Contains("WHERE") && await publicados.CountAsync() == 2, "Filtro diferido incorrecto.");
            var pagina = await contexto.Articulos.AsNoTracking().OrderBy(a => a.Id).Skip(1).Take(1).Select(a => a.Titulo).SingleAsync();
            Comprobaciones.Exigir(pagina == "Borrador", "Paginacion incorrecta.");
            var grupo = await contexto.Articulos.GroupBy(a => a.Categoria.Nombre)
                .Select(g => new { Categoria = g.Key, Total = g.Count(), Promedio = g.Average(a => a.TiempoLectura) }).SingleAsync();
            Comprobaciones.Exigir(grupo.Total == 3 && grupo.Promedio > 13 && grupo.Promedio < 14, "Agrupacion incorrecta.");
            var autores = await (from a in contexto.Articulos join u in contexto.Usuarios on a.AutorId equals u.Id
                select new { Articulo = a.Titulo, Autor = u.Nombre }).ToListAsync();
            Comprobaciones.Exigir(autores.Count == 3 && autores.All(a => a.Autor == "Usuario Demo"), "Join incorrecto.");
            var categoria = await contexto.Categorias.AsNoTracking().Include(c => c.Articulos.Where(a => a.Publicado)).SingleAsync();
            Comprobaciones.Exigir(categoria.Articulos.Count == 2, "Include filtrado incorrecto.");
            Comprobaciones.Exigir(await contexto.Articulos.SelectMany(a => a.Etiquetas).CountAsync() == 1, "SelectMany incorrecto.");

            var separado = await contexto.Articulos.AsNoTracking().OrderBy(a => a.Id).FirstAsync();
            contexto.Attach(separado);
            separado.Contenido = "Contenido actualizado con Attach";
            contexto.Entry(separado).Property(a => a.Contenido).IsModified = true;
            await contexto.SaveChangesAsync();
            contexto.ChangeTracker.Clear();
            Comprobaciones.Exigir(await contexto.Articulos.AnyAsync(a => a.Contenido == separado.Contenido), "Attach no persistio el campo.");

            await using (var transaccion = await contexto.Database.BeginTransactionAsync())
            {
                var actualizados = await contexto.Articulos.Where(a => !a.Publicado).ExecuteUpdateAsync(s => s.SetProperty(a => a.Publicado, true));
                var borrados = await contexto.Articulos.Where(a => a.Titulo == "Borrador").ExecuteDeleteAsync();
                Comprobaciones.Exigir(actualizados == 1 && borrados == 1 && await contexto.Articulos.CountAsync() == 2, "Operaciones masivas incorrectas.");
                await transaccion.RollbackAsync();
            }
            Comprobaciones.Exigir(await contexto.Articulos.CountAsync() == 3 && await contexto.Articulos.CountAsync(a => a.Publicado) == 2, "Rollback incorrecto.");
            Console.WriteLine("LINQ, proyecciones, paginacion, GroupBy, Join, Include, SelectMany, Attach y operaciones masivas: correctos.");
        }
        finally { await contexto.Database.EnsureDeletedAsync(); }
    }
}
