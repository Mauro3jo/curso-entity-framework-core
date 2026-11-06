using Microsoft.EntityFrameworkCore;

namespace Blog;

public class BlogDbContext(DbContextOptions<BlogDbContext> opciones) : DbContext(opciones)
{
    public const string ConexionDemo = "Server=(localdb)\\CodexEfCoreCurso;Database=EfCoreCursoBlog04;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
    public static DbContextOptions<BlogDbContext> OpcionesDemo() => new DbContextOptionsBuilder<BlogDbContext>().UseSqlServer(ConexionDemo).Options;
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<PerfilUsuario> Perfiles => Set<PerfilUsuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Articulo> Articulos => Set<Articulo>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Usuario>().HasOne(u => u.Perfil).WithOne(p => p.Usuario)
            .HasForeignKey<PerfilUsuario>(p => p.UsuarioId).OnDelete(DeleteBehavior.Cascade);
        modelo.Entity<Articulo>().HasOne(a => a.Categoria).WithMany(c => c.Articulos)
            .HasForeignKey(a => a.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        modelo.Entity<Articulo>().HasOne(a => a.Autor).WithMany()
            .HasForeignKey(a => a.AutorId).OnDelete(DeleteBehavior.Restrict);
        modelo.Entity<Comentario>().HasOne(c => c.Autor).WithMany()
            .HasForeignKey(c => c.AutorId).OnDelete(DeleteBehavior.Restrict);
        modelo.Entity<Articulo>().HasMany(a => a.Comentarios).WithOne(c => c.Articulo)
            .HasForeignKey(c => c.ArticuloId).OnDelete(DeleteBehavior.Cascade);
        modelo.Entity<Articulo>().HasMany(a => a.Etiquetas).WithMany(e => e.Articulos)
            .UsingEntity("ArticuloEtiquetas");
    }
}
