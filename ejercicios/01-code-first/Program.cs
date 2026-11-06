using CodeFirst.Datos;
using Microsoft.EntityFrameworkCore;

// Generar el esquema no abre una conexión ni modifica una base de datos.
using var contexto = new CursoDbContext();
Console.WriteLine(contexto.Database.GenerateCreateScript());
