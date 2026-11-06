using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blog;

public class Usuario
{
    public int Id { get; set; }
    [Required, MaxLength(100)] public string Nombre { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = string.Empty;
    public PerfilUsuario? Perfil { get; set; }
}

public class PerfilUsuario
{
    [Key] public int UsuarioId { get; set; }
    [MaxLength(250)] public string Biografia { get; set; } = string.Empty;
    public Usuario Usuario { get; set; } = null!;
}

public class Categoria
{
    public int Id { get; set; }
    [Required, MaxLength(100)] public string Nombre { get; set; } = string.Empty;
    public List<Articulo> Articulos { get; set; } = [];
}

public class Articulo
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Titulo { get; set; } = string.Empty;
    [Required, MaxLength(1000)] public string Contenido { get; set; } = string.Empty;
    [Range(1, 120)] public int TiempoLectura { get; set; }
    [Precision(12, 2)] public decimal PrecioPublicidad { get; set; }
    public bool Publicado { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public int AutorId { get; set; }
    public Usuario Autor { get; set; } = null!;
    [Timestamp] public byte[] Version { get; set; } = [];
    public List<Comentario> Comentarios { get; set; } = [];
    public List<Etiqueta> Etiquetas { get; set; } = [];
}

public class Comentario
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Texto { get; set; } = string.Empty;
    public int ArticuloId { get; set; }
    public Articulo Articulo { get; set; } = null!;
    public int AutorId { get; set; }
    public Usuario Autor { get; set; } = null!;
}

public class Etiqueta
{
    public int Id { get; set; }
    [Required, MaxLength(50)] public string Nombre { get; set; } = string.Empty;
    public List<Articulo> Articulos { get; set; } = [];
}
