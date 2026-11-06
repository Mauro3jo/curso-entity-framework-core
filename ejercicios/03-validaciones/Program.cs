using System.ComponentModel.DataAnnotations;
using Validaciones;

var casos = new (object Modelo, bool Valido, string? Campo)[]
{
    (new Articulo { Titulo = "EF Core", Contenido = "Ejemplo", TiempoLectura = 1 }, true, null),
    (new Articulo { Titulo = new string('a', 200), Contenido = "Ejemplo", TiempoLectura = 120 }, true, null),
    (new Articulo { Titulo = "", Contenido = "Ejemplo", TiempoLectura = 10 }, false, "Titulo"),
    (new Articulo { Titulo = new string('a', 201), Contenido = "Ejemplo", TiempoLectura = 10 }, false, "Titulo"),
    (new Articulo { Titulo = "EF", Contenido = new string('a', 1001), TiempoLectura = 10 }, false, "Contenido"),
    (new Articulo { Titulo = "EF", Contenido = "Ejemplo", TiempoLectura = 0 }, false, "TiempoLectura"),
    (new Articulo { Titulo = "EF", Contenido = "Ejemplo", TiempoLectura = 121 }, false, "TiempoLectura"),
    (new Usuario { Nombre = "Demo", Email = "demo@example.com", Alias = "demo_01" }, true, null),
    (new Usuario { Nombre = "Demo", Email = "no-es-correo", Alias = "demo" }, false, "Email"),
    (new Usuario { Nombre = "Demo", Email = "demo@example.com", Alias = "con espacio" }, false, "Alias"),
    (new Usuario { Nombre = "Demo", Email = "demo@example.com", Alias = "con\ttabulador" }, false, "Alias"),
    (new Usuario { Nombre = "Demo", Email = "demo@example.com", Alias = "" }, false, "Alias")
};

foreach (var caso in casos)
{
    var errores = new List<ValidationResult>();
    var valido = Validator.TryValidateObject(caso.Modelo, new ValidationContext(caso.Modelo), errores, true);
    if (valido != caso.Valido || (caso.Campo is not null && !errores.Any(e => e.MemberNames.Contains(caso.Campo))))
        throw new InvalidOperationException($"Resultado inesperado para {caso.Modelo.GetType().Name}: {caso.Campo}");
}

Console.WriteLine($"{casos.Length} comprobaciones de validacion correctas.");
