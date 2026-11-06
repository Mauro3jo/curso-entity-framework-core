# 02. Database First

El material `CursoEFCoreDatabaseFirst.rar` usa una tabla `categoria` con nombre, descripción opcional y estado activo. Aquí se armó una base nueva con esa tabla y dos categorías ficticias para generar el modelo desde SQL Server. El respaldo original queda aparte.

## Compilar y ver el esquema

Desde la raíz:

```powershell
dotnet tool restore
dotnet restore ejercicios/02-database-first/DatabaseFirst.csproj
dotnet build ejercicios/02-database-first/DatabaseFirst.csproj --no-restore
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj --no-build
```

Sin argumentos, la consola imprime el SQL del modelo sin conectarse a una base.

## Crear la base de ejemplo

Requiere Windows, SQL Server LocalDB y `sqlcmd`:

```powershell
SqlLocalDB create CodexEfCoreCurso 17.0
SqlLocalDB start CodexEfCoreCurso
sqlcmd -S "(localdb)\CodexEfCoreCurso" -E -b -i ejercicios/02-database-first/crear-base-demo.sql
```

Si la instancia ya existe, omitir su creación. El script se detiene con `-b` si encuentra la base `EfCoreCursoDatabaseFirst02`: no la borra ni la reemplaza.

```powershell
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj --no-build -- --consultar
```

La consulta devuelve `Programacion` y `Bases de datos`.

## Generar las clases desde la base

```powershell
dotnet ef dbcontext scaffold "Server=(localdb)\CodexEfCoreCurso;Database=EfCoreCursoDatabaseFirst02;Integrated Security=True;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project ejercicios/02-database-first --context CategoriasDbContext --context-dir Datos --output-dir Models --namespace DatabaseFirst.Models --context-namespace DatabaseFirst.Datos --data-annotations --no-onconfiguring --no-pluralize --table dbo.categoria
```

Las clases ya están incluidas. El comando omite `--force` para no sobrescribirlas por accidente. Revisar los cambios antes de regenerar sobre archivos editados.

`--no-onconfiguring` deja la conexión fuera del contexto generado. `--no-pluralize` evita que el nombre se convierta automáticamente en `Categorium`. El filtro de tabla limita la importación a `categoria`.

La conexión predeterminada es local, con autenticación integrada. Se puede cambiar mediante `EFCORE_DATABASEFIRST_SQLSERVER` en el entorno. La opción de confiar en el certificado es para esta práctica local.

## Comprobado

Se crearon una tabla y dos registros ficticios en una instancia separada. Las clases se generaron desde esa base; compilaron sin errores ni advertencias y la consulta devolvió ambas filas. Repetir el script dejó intactos esos datos y devolvió el error esperado. Una opción de consola desconocida termina con código 1 antes de conectarse.

Referencia: [ingeniería inversa con EF Core](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/).
