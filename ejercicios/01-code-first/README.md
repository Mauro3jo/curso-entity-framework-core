# 01. Code First: modelos, contexto y migración

Ejercicio reconstruido para repasar los fundamentos del curso de Entity Framework Core. El código se escribió de nuevo para este repositorio; no es una recuperación de los archivos originales ni una copia del proyecto descargado del instructor.

## Objetivo

Convertir clases C# en un esquema SQL Server con EF Core. `Categoria` y `Usuario` tienen una clave primaria `Id`; el contexto expone ambas entidades mediante `DbSet`. No hay relaciones ni operaciones CRUD en esta primera etapa.

La reconstrucción usa una consola para observar el SQL directamente. El material de referencia del curso usa una aplicación web; los controladores y las vistas se abordarán en etapas posteriores. `Edad` es un entero en este ejercicio. El proyecto fija .NET 10 y EF Core 10.0.14 estable, en lugar de los paquetes RC1 de las descargas.

Se fija también `System.Security.Cryptography.Pkcs` 10.0.14 para evitar la versión transitiva 9.0.11 señalada por NuGet durante la restauración. Véanse los avisos de Microsoft [CVE-2026-70162](https://github.com/advisories/GHSA-2fmv-rv2c-59x7) y [CVE-2026-84008](https://github.com/advisories/GHSA-846f-f764-76mx).

## Ejecutar sin una base de datos

Requisito: SDK .NET 10. Desde la raíz del repositorio:

```powershell
dotnet tool restore
dotnet restore ejercicios/01-code-first/CodeFirst.csproj
dotnet build ejercicios/01-code-first/CodeFirst.csproj --no-restore
dotnet run --project ejercicios/01-code-first/CodeFirst.csproj --no-build
```

La consola imprime las sentencias `CREATE TABLE` de `Categorias` y `Usuarios`. Genera texto SQL: no crea la base, no inserta registros y no necesita un servidor SQL activo.

## Revisar la migración

```powershell
dotnet ef migrations has-pending-model-changes --project ejercicios/01-code-first
dotnet ef migrations script 0 Inicial --project ejercicios/01-code-first --output ejercicios/01-code-first/esquema-inicial.sql
```

La migración inicial y su snapshot están versionados en `Migrations/`. El script permite revisar el esquema y la tabla de historial de migraciones sin conectarse a SQL Server.

El script `esquema-inicial.sql` conserva la primera versión del esquema. El modelo actual incorpora además una descripción opcional de categoría mediante una segunda migración. Véase [Evolución del modelo](evolucion-del-modelo.md) para generar el cambio incremental y comprender su reversión.

## Crear una base de práctica, opcional

La conexión predeterminada usa SQL Server LocalDB y autenticación integrada de Windows. `TrustServerCertificate=True` se limita a esta conexión local de práctica. La generación de SQL funciona también sin LocalDB; aplicar la migración requiere una instancia disponible.

Si se dispone de LocalDB y se desea crear exclusivamente la base nueva `EfCoreAprendizaje01`:

```powershell
dotnet ef database update --project ejercicios/01-code-first
```

Ese comando sí modifica una base de datos y no forma parte de las comprobaciones sin conexión. Para otra instancia se puede definir `EFCORE_APRENDIZAJE_SQLSERVER` en el entorno local. No guardar contraseñas ni conexiones reales en los archivos del repositorio.

## Verificación de esta reconstrucción

- Compilación con SDK 10.0.403: cero errores y cero advertencias.
- EF Core informa que no hay cambios pendientes entre el modelo y la migración.
- La ejecución de la consola genera las tablas `Categorias` y `Usuarios`.
- Se generó `esquema-inicial.sql` a partir de la migración. No se aplicó contra una base de datos.

No se probó la creación real de la base, su restauración ni operaciones CRUD; pertenecen a pasos posteriores.

## Fuentes y alcance

- [Curso de render2web en Udemy](https://ucic.udemy.com/course/curso-entity-framework-core/): referencia de los temas y nombres de entidades.
- [Primeros pasos con EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app): modelos y contexto.
- [Aplicación de migraciones](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying): generación y revisión de scripts.

Las descargas originales del instructor y el respaldo de datos no se incluyen. Este ejercicio no implementa validaciones de negocio, relaciones, pantallas web ni un sistema listo para producción.
