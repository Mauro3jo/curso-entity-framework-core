using System.ComponentModel.DataAnnotations;

namespace Validaciones;

public class Articulo
{
    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Contenido { get; set; } = string.Empty;

    [Range(1, 120), Display(Name = "Tiempo de lectura (minutos)")]
    public int TiempoLectura { get; set; }
}

public class Usuario
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30), NoEspacios]
    public string Alias { get; set; } = string.Empty;
}

public sealed class NoEspaciosAttribute : ValidationAttribute
{
    public NoEspaciosAttribute() => ErrorMessage = "El alias no admite caracteres de espacio.";

    public override bool IsValid(object? value) => value is null
        || value is string texto && !texto.Any(char.IsWhiteSpace);
}
