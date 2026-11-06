using Academia;
using Microsoft.EntityFrameworkCore;

if (args is ["--verificar-sql"])
    await ConsultasSql.VerificarAsync();
else if (args is ["--verificar"])
    await Pruebas.FluentAsync();
else
{
    using var contexto = AcademiaDbContext.Crear("EfCoreCursoAcademia05");
    Console.WriteLine(contexto.Database.GenerateCreateScript());
}
