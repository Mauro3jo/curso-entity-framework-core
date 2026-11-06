using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LibrosMySql;

public class Libro
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Titulo { get; set; } = string.Empty;
    [Required, MaxLength(150)] public string Autor { get; set; } = string.Empty;
    [Range(1450, 2100)] public int AnioPublicacion { get; set; }
    public decimal Precio { get; set; }
    public bool Disponible { get; set; }
}

public class LibrosDbContext(DbContextOptions<LibrosDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Libro> Libros => Set<Libro>();
    protected override void OnModelCreating(ModelBuilder modelo) => modelo.Entity<Libro>().Property(l => l.Precio).HasPrecision(10, 2);
}
