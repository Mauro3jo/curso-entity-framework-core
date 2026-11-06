using DatabaseFirst.Datos;
using Microsoft.EntityFrameworkCore;

var consultar = args is ["--consultar"];
if (args.Length > 0 && !consultar)
{
    Console.Error.WriteLine("Uso: dotnet run -- [--consultar]");
    return 1;
}

var conexion = Environment.GetEnvironmentVariable("EFCORE_DATABASEFIRST_SQLSERVER")
    ?? "Server=(localdb)\\CodexEfCoreCurso;Database=EfCoreCursoDatabaseFirst02;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
var opciones = new DbContextOptionsBuilder<CategoriasDbContext>()
    .UseSqlServer(conexion)
    .Options;
using var contexto = new CategoriasDbContext(opciones);

if (consultar)
{
    var categorias = await contexto.Categoria.AsNoTracking()
        .OrderBy(categoria => categoria.Idcategoria)
        .ToListAsync();

    foreach (var categoria in categorias)
        Console.WriteLine($"{categoria.Idcategoria}: {categoria.Nombre} | Activa: {categoria.Activo}");
}
else
{
    // La generación del esquema no abre una conexión.
    Console.WriteLine(contexto.Database.GenerateCreateScript());
}

return 0;
