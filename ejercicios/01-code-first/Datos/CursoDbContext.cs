using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Datos;

public class CursoDbContext : DbContext
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var conexion = Environment.GetEnvironmentVariable("EFCORE_APRENDIZAJE_SQLSERVER")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=EfCoreAprendizaje01;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(conexion);
    }
}
