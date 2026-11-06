using Blog;
using Microsoft.EntityFrameworkCore;

if (args is ["--verificar"])
{
    await Comprobaciones.RelacionesAsync();
    return;
}

var opciones = BlogDbContext.OpcionesDemo();
if (args is ["--consultas"])
{
    await Consultas.VerificarAsync();
    return;
}
if (args is ["--crear-demo"])
{
    await using var contexto = new BlogDbContext(opciones);
    if (await contexto.Database.CanConnectAsync())
        throw new InvalidOperationException("La base ya existe; no se modifica.");
    await contexto.Database.EnsureCreatedAsync();
    await DatosDemo.InsertarAsync(contexto);
    Console.WriteLine("Base nueva de demostracion creada.");
    return;
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(BlogDbContext.ConexionDemo));
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapGet("/", () => "Blog de practica: modelos relacionales de EF Core.");
app.MapControllerRoute("default", "{controller=Articulos}/{action=Index}/{id?}");
app.Run();
