# 05. Fluent API

Modelo académico reconstruido a partir de los temas de `FluentAPI_EFCore10.rar`: estudiantes, perfiles, cursos, clases e inscripciones.

```powershell
dotnet run --project ejercicios/05-fluent-api/Academia.csproj
dotnet run --project ejercicios/05-fluent-api/Academia.csproj -- --verificar
```

Sin argumentos imprime el SQL sin conectarse. `--verificar` necesita la instancia LocalDB del curso; crea una base temporal propia y la elimina al terminar.

La configuración muestra claves simples y compuestas, índice único de correo, restricción de edad, relaciones, un objeto `Direccion` con `OwnsOne`, conversión de un enum a texto, precisión decimal, fecha por defecto y un curso ficticio con `HasData`. La columna calculada aplica un factor de ejemplo de 1,21 al costo; no representa una regla fiscal real.

Las pruebas leen los valores guardados y comprueban que SQL Server rechace correos duplicados, edades fuera de rango, inscripciones duplicadas y referencias inexistentes. Los rechazos se identifican por los códigos de SQL Server, no por cualquier excepción.

Las bases y sus datos son de práctica. Los archivos del profesor se usan como referencia; este proyecto no es una recuperación de los originales.
