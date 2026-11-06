using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Academia;

public class ReporteInscripciones
{
    public int CursoId { get; set; }
    public string Curso { get; set; } = string.Empty;
    public int DuracionHoras { get; set; }
    public bool Activo { get; set; }
    public int TotalInscritos { get; set; }
    public double? EdadPromedio { get; set; }
}

public static class ConsultasSql
{
    public static async Task VerificarAsync() => await Pruebas.EnBaseTemporalAsync(async contexto =>
    {
        var script = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "rutinas.sql"));
        // Solo se divide el archivo de ejemplo del proyecto; no es un parser general de SQL.
        foreach (var lote in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            if (!string.IsNullOrWhiteSpace(lote)) await contexto.Database.ExecuteSqlRawAsync(lote);

        var vacio = await contexto.Reportes.AsNoTracking().SingleAsync();
        Pruebas.Exigir(vacio.TotalInscritos == 0 && vacio.EdadPromedio is null, "La vista debe conservar cursos sin inscriptos.");
        var nombre = "Alumno de ejemplo";
        var email = "sql-demo@example.com";
        var edad = 20;
        await contexto.Database.ExecuteSqlInterpolatedAsync($"EXEC Academico.InsertarEstudiante @Nombre={nombre}, @Email={email}, @Edad={edad}");
        var id = await contexto.Estudiantes.Select(e => e.Id).SingleAsync();
        contexto.Inscripciones.Add(new Inscripcion { CursoId = 1, EstudianteId = id });
        await contexto.SaveChangesAsync();
        contexto.ChangeTracker.Clear();
        var reporte = await contexto.Reportes.AsNoTracking().SingleAsync();
        Pruebas.Exigir(reporte.TotalInscritos == 1 && reporte.EdadPromedio == 20, "Conteo o promedio incorrecto.");
        var minimo = 20;
        var cursos = await contexto.Reportes.FromSqlInterpolated($"EXEC Academico.CursosPorDuracion @MinimoHoras={minimo}").AsNoTracking().ToListAsync();
        Pruebas.Exigir(cursos.Count == 1, "El procedimiento no devolvio el curso esperado.");
        minimo = 41;
        var fueraDeRango = await contexto.Reportes.FromSqlInterpolated($"EXEC Academico.CursosPorDuracion @MinimoHoras={minimo}").ToListAsync();
        Pruebas.Exigir(fueraDeRango.Count == 0, "El parametro del filtro no se aplico.");
        edad = 22;
        await contexto.Database.ExecuteSqlInterpolatedAsync($"EXEC Academico.ActualizarEdad @Id={id}, @Edad={edad}");
        Pruebas.Exigir((await contexto.Reportes.SingleAsync()).EdadPromedio == 22, "La actualizacion no se reflejo en la vista.");
        nombre = "Alumno con 'apostrofo'";
        email = "sql-comillas@example.com";
        await contexto.Database.ExecuteSqlInterpolatedAsync($"EXEC Academico.InsertarEstudiante @Nombre={nombre}, @Email={email}, @Edad={edad}");
        Pruebas.Exigir(await contexto.Estudiantes.CountAsync(e => e.Nombre == nombre) == 1, "El nombre parametrizado no se conservo.");
        Console.WriteLine("Vista, entidad sin clave y procedimientos parametrizados de consulta, insercion y actualizacion: correctos.");
    });
}
