# 05. Fluent API

Estudiantes, perfiles, cursos, clases e inscripciones, con la configuración en `AcademiaDbContext`.

```powershell
dotnet run --project ejercicios/05-fluent-api/Academia.csproj
dotnet run --project ejercicios/05-fluent-api/Academia.csproj -- --verificar
dotnet run --project ejercicios/05-fluent-api/Academia.csproj -- --verificar-sql
```

Sin argumentos imprime el esquema. Las pruebas necesitan la instancia LocalDB del curso; crean bases temporales y las eliminan al terminar.

Incluye claves compuestas, índice único, restricción de edad, `OwnsOne`, conversión de enum, valores por defecto y `HasData`. La columna calculada usa un factor ficticio de 1,21.

`--verificar` comprueba también los rechazos por duplicados y referencias inválidas. [Vistas y procedimientos almacenados](sql.md).
