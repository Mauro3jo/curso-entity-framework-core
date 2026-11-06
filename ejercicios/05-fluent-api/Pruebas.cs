using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Academia;

public static class Pruebas
{
    public static void Exigir(bool condicion, string mensaje)
    {
        if (!condicion) throw new InvalidOperationException(mensaje);
    }

    public static async Task EnBaseTemporalAsync(Func<AcademiaDbContext, Task> comprobar)
    {
        await using var contexto = AcademiaDbContext.Crear("EfCoreCursoAcademia_" + Guid.NewGuid().ToString("N"));
        Exigir(await contexto.Database.EnsureCreatedAsync(), "Se requiere una base temporal nueva.");
        try { await comprobar(contexto); }
        finally { await contexto.Database.EnsureDeletedAsync(); }
    }

    public static async Task FluentAsync() => await EnBaseTemporalAsync(async contexto =>
    {
        var alumno = new Estudiante { Nombre = "Alumno Demo", Email = "alumno@example.com", Edad = 20,
            Direccion = new Direccion { Calle = "Calle de ejemplo", Ciudad = "Ciudad Demo" },
            Perfil = new PerfilAcademico { Carrera = "Informatica" } };
        contexto.Estudiantes.Add(alumno);
        await contexto.SaveChangesAsync();
        var alumnoId = alumno.Id;
        Exigir(alumno.FechaRegistro != default, "No se genero la fecha por defecto.");
        contexto.Inscripciones.Add(new Inscripcion { CursoId = 1, EstudianteId = alumnoId });
        contexto.Clases.Add(new Clase { CursoId = 1, Nombre = "Introduccion" });
        await contexto.SaveChangesAsync();
        contexto.ChangeTracker.Clear();
        Exigir((await contexto.Cursos.SingleAsync()).CostoConImpuesto == 121m, "Columna calculada incorrecta.");
        Exigir((await contexto.Estudiantes.SingleAsync()).Direccion.Ciudad == "Ciudad Demo", "Owned type incorrecto.");
        Exigir(await contexto.Database.SqlQueryRaw<string>("SELECT [Nivel] AS [Value] FROM [Academico].[Cursos] WHERE [Id]=1").SingleAsync() == "Basico", "Conversion de enum incorrecta.");
        await RechazarAsync(contexto, () => contexto.Estudiantes.Add(new Estudiante { Nombre = "Otro", Email = "alumno@example.com", Edad = 25 }), 2601, 2627);
        await RechazarAsync(contexto, () => contexto.Estudiantes.Add(new Estudiante { Nombre = "Menor", Email = "menor@example.com", Edad = 5 }), 547);
        await RechazarAsync(contexto, () => contexto.Inscripciones.Add(new Inscripcion { CursoId = 1, EstudianteId = alumnoId }), 2627);
        await RechazarAsync(contexto, () => contexto.Inscripciones.Add(new Inscripcion { CursoId = 999999, EstudianteId = alumnoId }), 547);
        Console.WriteLine("Fluent API: claves, indice unico, check, relaciones, owned type, enum, valores por defecto, seed y columna calculada correctos.");
    });

    private static async Task RechazarAsync(AcademiaDbContext contexto, Action agregar, params int[] codigos)
    {
        contexto.ChangeTracker.Clear();
        agregar();
        try { await contexto.SaveChangesAsync(); }
        catch (DbUpdateException error) when (error.InnerException is SqlException sql && codigos.Contains(sql.Number))
        { contexto.ChangeTracker.Clear(); return; }
        throw new InvalidOperationException("La restriccion esperada no rechazo el registro.");
    }
}
