namespace Blog;

public static class DatosDemo
{
    public static async Task InsertarAsync(BlogDbContext contexto)
    {
        var autor = new Usuario { Nombre = "Usuario Demo", Email = "demo@example.com", Perfil = new PerfilUsuario { Biografia = "Perfil ficticio" } };
        contexto.Articulos.Add(new Articulo
        {
            Titulo = "Relaciones con EF Core", Contenido = "Articulo ficticio de practica.", TiempoLectura = 5,
            Categoria = new Categoria { Nombre = "EF Core" }, Autor = autor, Publicado = true,
            Etiquetas = [new Etiqueta { Nombre = "dotnet" }],
            Comentarios = [new Comentario { Texto = "Comentario ficticio", Autor = autor }]
        });
        await contexto.SaveChangesAsync();
    }
}
