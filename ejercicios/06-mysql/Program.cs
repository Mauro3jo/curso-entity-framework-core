using LibrosMySql;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

if (args.Length == 0)
{
    var opciones = new DbContextOptionsBuilder<LibrosDbContext>()
        .UseMySQL("Server=127.0.0.1;Port=13316;Database=efcore_libros_demo;User ID=curso").Options;
    using var modelo = new LibrosDbContext(opciones);
    var sql = modelo.Database.GenerateCreateScript();
    if (!sql.Contains("Libros") || !sql.Contains("decimal(10,2)", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("El esquema MySQL no contiene lo esperado.");
    Console.WriteLine(sql);
    return;
}

if (args is not ["--verificar"])
    throw new ArgumentException("Usar sin argumentos para generar SQL o --verificar para probar MySQL real.");
var original = Environment.GetEnvironmentVariable("EFCORE_MYSQL_CONNECTION")
    ?? throw new InvalidOperationException("Definir EFCORE_MYSQL_CONNECTION localmente para un servidor MySQL de pruebas.");
var cadena = new MySqlConnectionStringBuilder(original) { Database = string.Empty };
await using (var servidor = new MySqlConnection(cadena.ConnectionString))
{
    await servidor.OpenAsync();
    if (servidor.ServerVersion.Contains("MariaDB", StringComparison.OrdinalIgnoreCase)
        || !Version.TryParse(servidor.ServerVersion.Split('-')[0], out var version) || version.Major < 8)
        throw new NotSupportedException("Esta practica usa el proveedor Oracle para MySQL; no se verifica contra MariaDB.");
}

cadena.Database = "efcore_curso_checks_" + Guid.NewGuid().ToString("N");
var configuracion = new DbContextOptionsBuilder<LibrosDbContext>().UseMySQL(cadena.ConnectionString).Options;
await using var contexto = new LibrosDbContext(configuracion);
var creada = await contexto.Database.EnsureCreatedAsync();
if (!creada) throw new InvalidOperationException("Se requiere una base temporal nueva.");
try
{
    var libro = new Libro { Titulo = "Libro ficticio", Autor = "Autor Demo", AnioPublicacion = 2026, Precio = 123.45m, Disponible = true };
    contexto.Libros.Add(libro);
    await contexto.SaveChangesAsync();
    var id = libro.Id;
    contexto.ChangeTracker.Clear();
    var leido = await contexto.Libros.SingleAsync(l => l.Id == id);
    if (leido.Precio != 123.45m || !leido.Disponible) throw new InvalidOperationException("Lectura incorrecta.");
    leido.Precio = 99.90m;
    leido.Disponible = false;
    await contexto.SaveChangesAsync();
    contexto.ChangeTracker.Clear();
    var editado = await contexto.Libros.SingleAsync(l => l.Id == id);
    if (editado.Precio != 99.90m || editado.Disponible) throw new InvalidOperationException("Actualizacion incorrecta.");
    contexto.Libros.Remove(editado);
    await contexto.SaveChangesAsync();
    if (await contexto.Libros.AnyAsync()) throw new InvalidOperationException("Borrado incorrecto.");
    Console.WriteLine("CRUD y precision decimal en MySQL real: correctos.");
}
finally { await contexto.Database.EnsureDeletedAsync(); }
