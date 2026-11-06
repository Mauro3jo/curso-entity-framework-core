# Segunda migración

`AgregarDescripcionCategoria` agrega `Descripcion` como columna opcional. Las categorías anteriores pueden dejarla en `NULL`; el esquema inicial se conserva en `esquema-inicial.sql`.

```powershell
dotnet build ejercicios/01-code-first/CodeFirst.csproj
dotnet ef migrations script Inicial AgregarDescripcionCategoria --idempotent --project ejercicios/01-code-first --output ejercicios/01-code-first/evolucion-categoria.sql
```

Generar el script no lo ejecuta. Espera una base con la migración `Inicial` y consulta el historial de EF para omitir el cambio si ya está aplicado.

Para ver el SQL de reversión:

```powershell
dotnet ef migrations script AgregarDescripcionCategoria Inicial --project ejercicios/01-code-first
```

La reversión elimina la columna y perdería las descripciones guardadas. Se revisó el SQL en ambos sentidos, pero no se aplicó sobre una base con datos.
