using Microsoft.EntityFrameworkCore;

namespace Academia;

public class AcademiaDbContext(DbContextOptions<AcademiaDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Clase> Clases => Set<Clase>();
    public DbSet<PerfilAcademico> Perfiles => Set<PerfilAcademico>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
    public DbSet<ReporteInscripciones> Reportes => Set<ReporteInscripciones>();

    public static AcademiaDbContext Crear(string nombre) => new(new DbContextOptionsBuilder<AcademiaDbContext>()
        .UseSqlServer($"Server=(localdb)\\CodexEfCoreCurso;Database={nombre};Integrated Security=True;Encrypt=True;TrustServerCertificate=True").Options);

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.HasDefaultSchema("Academico");
        modelo.Entity<ReporteInscripciones>().HasNoKey().ToView("vw_Inscripciones", "Academico");
        modelo.Entity<Estudiante>(entidad =>
        {
            entidad.ToTable("Estudiantes", tabla => tabla.HasCheckConstraint("CK_Estudiantes_Edad", "[Edad] BETWEEN 18 AND 100"));
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entidad.Property(e => e.Email).IsRequired().HasMaxLength(150).IsUnicode(false);
            entidad.HasIndex(e => e.Email).IsUnique();
            entidad.Property(e => e.FechaRegistro).HasDefaultValueSql("SYSUTCDATETIME()");
            entidad.OwnsOne(e => e.Direccion, direccion =>
            {
                direccion.Property(d => d.Calle).HasColumnName("DireccionCalle").HasMaxLength(120);
                direccion.Property(d => d.Ciudad).HasColumnName("DireccionCiudad").HasMaxLength(80);
            });
            entidad.HasOne(e => e.Perfil).WithOne(p => p.Estudiante).HasForeignKey<PerfilAcademico>(p => p.EstudianteId);
        });
        modelo.Entity<PerfilAcademico>().HasKey(p => p.EstudianteId);
        modelo.Entity<PerfilAcademico>().Property(p => p.Carrera).HasMaxLength(100);
        modelo.Entity<Curso>(entidad =>
        {
            entidad.ToTable("Cursos", tabla => tabla.HasCheckConstraint("CK_Cursos_Duracion", "[DuracionHoras] > 0"));
            entidad.Property(c => c.Titulo).IsRequired().HasMaxLength(150);
            entidad.Property(c => c.Activo).HasDefaultValue(true);
            entidad.Property(c => c.Nivel).HasConversion<string>().HasMaxLength(20);
            entidad.Property(c => c.Costo).HasPrecision(12, 2);
            entidad.Property(c => c.CostoConImpuesto).HasPrecision(12, 2)
                .HasComputedColumnSql("CONVERT(decimal(12,2), [Costo] * 1.21)", stored: true);
            entidad.HasData(new Curso { Id = 1, Titulo = "EF Core Demo", DuracionHoras = 40, Activo = true, Nivel = NivelEducativo.Basico, Costo = 100 });
        });
        modelo.Entity<Clase>().Property(c => c.Nombre).HasMaxLength(150);
        modelo.Entity<Clase>().HasOne(c => c.Curso).WithMany(c => c.Clases).HasForeignKey(c => c.CursoId);
        modelo.Entity<Inscripcion>(entidad =>
        {
            entidad.HasKey(i => new { i.EstudianteId, i.CursoId });
            entidad.Property(i => i.Fecha).HasDefaultValueSql("SYSUTCDATETIME()");
            entidad.HasOne(i => i.Estudiante).WithMany(e => e.Inscripciones).HasForeignKey(i => i.EstudianteId);
            entidad.HasOne(i => i.Curso).WithMany(c => c.Inscripciones).HasForeignKey(i => i.CursoId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
