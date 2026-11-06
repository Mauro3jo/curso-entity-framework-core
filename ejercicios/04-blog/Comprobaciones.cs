using Microsoft.EntityFrameworkCore;

namespace Blog;

public static class Comprobaciones
{
    public static void Exigir(bool condicion, string mensaje)
    {
        if (!condicion) throw new InvalidOperationException(mensaje);
    }

    public static async Task RelacionesAsync()
    {
        // Nombre generado aquí; solo se elimina esta base temporal creada por la comprobación.
        var nombre = "EfCoreCursoChecks_" + Guid.NewGuid().ToString("N");
        var opciones = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlServer($"Server=(localdb)\\CodexEfCoreCurso;Database={nombre};Integrated Security=True;Encrypt=True;TrustServerCertificate=True")
            .Options;
        await using var contexto = new BlogDbContext(opciones);
        var creada = await contexto.Database.EnsureCreatedAsync();
        Exigir(creada, "No se creo una base temporal nueva.");
        try
        {
            await DatosDemo.InsertarAsync(contexto);
            contexto.ChangeTracker.Clear();
            var articulo = await contexto.Articulos.AsNoTracking()
                .Include(a => a.Categoria).Include(a => a.Autor).ThenInclude(u => u.Perfil)
                .Include(a => a.Comentarios).Include(a => a.Etiquetas).SingleAsync();
            Exigir(articulo.Autor.Perfil?.Biografia == "Perfil ficticio", "Relacion 1:1 incorrecta.");
            Exigir(articulo.Categoria.Nombre == "EF Core" && articulo.Comentarios.Count == 1, "Relacion 1:N incorrecta.");
            Exigir(articulo.Etiquetas.Single().Nombre == "dotnet", "Relacion N:N incorrecta.");
            contexto.Remove(await contexto.Categorias.SingleAsync());
            try
            {
                await contexto.SaveChangesAsync();
                throw new InvalidOperationException("La categoria con articulos no debe poder borrarse.");
            }
            catch (DbUpdateException) { contexto.ChangeTracker.Clear(); }
            contexto.Remove(await contexto.Articulos.SingleAsync());
            await contexto.SaveChangesAsync();
            Exigir(await contexto.Comentarios.CountAsync() == 0, "La cascada no elimino los comentarios.");
            Exigir(await contexto.Etiquetas.CountAsync() == 1, "La cascada no debe eliminar las etiquetas.");
            contexto.Remove(await contexto.Usuarios.SingleAsync());
            await contexto.SaveChangesAsync();
            Exigir(await contexto.Perfiles.CountAsync() == 0, "La cascada 1:1 no elimino el perfil.");
            Console.WriteLine("Relaciones 1:1, 1:N, N:N y borrado restringido/en cascada: correctos.");
        }
        finally
        {
            await contexto.Database.EnsureDeletedAsync();
        }
    }
}
