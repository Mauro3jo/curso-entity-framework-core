# Vistas y procedimientos almacenados

El curso incluye reportes de inscripciones y procedimientos para consultar, insertar y actualizar estudiantes. `rutinas.sql` reconstruye ejemplos de esos temas sobre la base académica de este proyecto.

```powershell
dotnet run --project ejercicios/05-fluent-api/Academia.csproj -- --verificar-sql
```

La prueba crea una base temporal propia, ejecuta el archivo SQL del proyecto y comprueba:

- Una vista que conserva cursos sin inscripciones mediante `LEFT JOIN`.
- Conteo de inscripciones y promedio de edad; el promedio queda en `NULL` cuando no hay estudiantes.
- Mapeo de la vista a una entidad sin clave con `HasNoKey` y `ToView`.
- Un procedimiento filtrado por duración, llamado con `FromSqlInterpolated`.
- Procedimientos de inserción y actualización con `ExecuteSqlInterpolated`.
- Conservación de un texto que contiene comillas, enviado como parámetro.

El resultado de `EXEC` se materializa antes de realizar más operaciones en memoria: no se agrega un `Where` SQL sobre una llamada no componible. Los datos de entrada se parametrizan; `ExecuteSqlRaw` se limita al archivo DDL fijo del ejercicio.

Los separadores `GO` se procesan únicamente para este archivo controlado. La prueba elimina la base temporal al terminar. No ejecuta estas rutinas contra la base del blog, el respaldo del curso ni una base de trabajo.
