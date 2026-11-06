# Vistas y procedimientos

```powershell
dotnet run --project ejercicios/05-fluent-api/Academia.csproj -- --verificar-sql
```

La prueba ejecuta `rutinas.sql` en una base temporal propia y la elimina al terminar.

La vista cuenta inscripciones y calcula la edad promedio; conserva los cursos sin inscriptos con `LEFT JOIN`. Se mapea con `HasNoKey` y `ToView`.

Los procedimientos consultan cursos por duración e insertan y actualizan estudiantes. Las llamadas usan `FromSqlInterpolated` y `ExecuteSqlInterpolated`, con parámetros. Los resultados de `EXEC` se materializan antes de seguir filtrando en memoria.

`ExecuteSqlRaw` se usa solo para el archivo DDL fijo del ejemplo. La separación de lotes por `GO` está hecha para ese archivo, no para cualquier script SQL.
