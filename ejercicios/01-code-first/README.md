# 01. Code First

`Categoria` y `Usuario` muestran cómo pasar de clases C# a tablas de SQL Server. El contexto expone ambas entidades y las migraciones guardan los cambios del esquema.

Es una reconstrucción del bloque inicial del curso. Para mirar el SQL sin armar una interfaz, este ejemplo usa una consola. `Edad` es un entero; las descargas del curso usan otra versión del modelo y paquetes RC1.

## Probar sin base de datos

Desde la raíz del repositorio:

```powershell
dotnet tool restore
dotnet restore ejercicios/01-code-first/CodeFirst.csproj
dotnet build ejercicios/01-code-first/CodeFirst.csproj --no-restore
dotnet run --project ejercicios/01-code-first/CodeFirst.csproj --no-build
```

La consola imprime el esquema actual. No abre una conexión ni ejecuta el SQL.

```powershell
dotnet ef migrations has-pending-model-changes --project ejercicios/01-code-first
dotnet ef migrations script 0 Inicial --project ejercicios/01-code-first --output ejercicios/01-code-first/esquema-inicial.sql
```

`esquema-inicial.sql` conserva la primera versión. La segunda migración agrega una descripción opcional: [ver el cambio y su reversión](evolucion-del-modelo.md).

## Aplicar en LocalDB, opcional

```powershell
dotnet ef database update --project ejercicios/01-code-first
```

Este comando sí crea o actualiza `EfCoreAprendizaje01` en `(localdb)\MSSQLLocalDB`. No se ejecutó durante las comprobaciones. La conexión usa autenticación integrada; para otro destino se puede definir `EFCORE_APRENDIZAJE_SQLSERVER` en el entorno. La opción de confiar en el certificado corresponde a la conexión local de práctica.

## Comprobado

Compilación sin errores ni advertencias, modelo sincronizado con las migraciones y SQL generado correctamente. No se probó la aplicación de estas migraciones sobre una base con registros.

El proyecto fija EF Core 10.0.14 y `System.Security.Cryptography.Pkcs` 10.0.14. Esta última referencia evita la versión transitiva señalada por NuGet en los avisos [CVE-2026-70162](https://github.com/advisories/GHSA-2fmv-rv2c-59x7) y [CVE-2026-84008](https://github.com/advisories/GHSA-846f-f764-76mx).

Referencias: [primeros pasos con EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app) y [migraciones](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying).
