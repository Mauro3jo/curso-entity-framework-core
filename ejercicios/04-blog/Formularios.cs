using System.ComponentModel.DataAnnotations;

namespace Blog;

public class FormularioArticulo
{
    [Required, MaxLength(200)] public string Titulo { get; set; } = string.Empty;
    [Required, MaxLength(1000)] public string Contenido { get; set; } = string.Empty;
    [Range(1, 120)] public int TiempoLectura { get; set; } = 5;
    [Range(1, int.MaxValue)] public int CategoriaId { get; set; }
    [Range(1, int.MaxValue)] public int AutorId { get; set; }
    public string? Version { get; set; }
}

public record ArticuloFila(int Id, string Titulo, string Categoria);
public record ListadoArticulos(List<ArticuloFila> Filas, string? Buscar, int Pagina);
public record DetalleArticulo(int Id, string Titulo, string Contenido, string Categoria, string Autor,
    List<string> Etiquetas, List<string> Comentarios, string Version);
