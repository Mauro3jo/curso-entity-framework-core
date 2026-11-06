# Evolución del modelo

La segunda migración agrega `Categoria.Descripcion` como `string?`, que SQL Server guarda en una columna opcional. Las categorías anteriores pueden mantener ese campo en `NULL`.

El historial queda así:

1. `Inicial`: crea las tablas `Categorias` y `Usuarios`.
2. `AgregarDescripcionCategoria`: agrega únicamente la nueva columna.

El snapshot guarda la versión actual del modelo. La migración inicial y su script se conservan.

## Generar el cambio

Desde la raíz, con las dependencias y herramientas restauradas:

```powershell
dotnet build ejercicios/01-code-first/CodeFirst.csproj --no-restore
dotnet ef migrations has-pending-model-changes --project ejercicios/01-code-first --no-build
dotnet ef migrations script Inicial AgregarDescripcionCategoria --idempotent --project ejercicios/01-code-first --no-build --output ejercicios/01-code-first/evolucion-categoria.sql
```

El script consulta el historial de EF para omitir la migración si ya fue aplicada. Espera una base con `Inicial`; no corrige cambios hechos manualmente en el esquema. Generar el archivo no ejecuta sus sentencias.

## Revisar la reversión

```powershell
dotnet ef migrations script AgregarDescripcionCategoria Inicial --project ejercicios/01-code-first --no-build
```

El SQL inverso elimina `Descripcion`. Aplicarlo sobre una base utilizada perdería los valores de esa columna.

Se comprobó la compilación, la coincidencia entre modelo y migración y el SQL en ambos sentidos. El avance no recrea tablas ni incluye borrado de registros. No se aplicó contra una base con datos; no se afirma haber probado una actualización real de esos registros.
