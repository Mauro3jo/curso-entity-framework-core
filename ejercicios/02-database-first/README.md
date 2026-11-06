# 02. Database First

Una tabla `categoria` con dos filas ficticias. El contexto y la entidad se generaron desde esa base, tomando como referencia `CursoEFCoreDatabaseFirst.rar`.

Para ver el esquema sin conectarse:

```powershell
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj
```

Para crear la base se necesitan Windows, LocalDB 17 y `sqlcmd`. Ejecutar desde la raíz:

```powershell
SqlLocalDB create CodexEfCoreCurso 17.0
SqlLocalDB start CodexEfCoreCurso
sqlcmd -S "(localdb)\CodexEfCoreCurso" -E -b -i ejercicios/02-database-first/crear-base-demo.sql
```

Omitir `create` si la instancia ya existe. El script se detiene si encuentra la base `EfCoreCursoDatabaseFirst02`; no la reemplaza.

```powershell
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj -- --consultar
```

La consulta devuelve las categorías `Programacion` y `Bases de datos`.

Para generar las clases:

```powershell
dotnet tool restore
dotnet ef dbcontext scaffold "Server=(localdb)\CodexEfCoreCurso;Database=EfCoreCursoDatabaseFirst02;Integrated Security=True;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project ejercicios/02-database-first --context CategoriasDbContext --context-dir Datos --output-dir Models --namespace DatabaseFirst.Models --context-namespace DatabaseFirst.Datos --data-annotations --no-onconfiguring --no-pluralize --table dbo.categoria
```

Las clases ya están incluidas. Revisar las diferencias antes de regenerarlas con `--force`.

`--no-onconfiguring` deja la conexión fuera del contexto; `--no-pluralize` conserva el nombre `Categoria`. La conexión de la consola se cambia con `EFCORE_DATABASEFIRST_SQLSERVER`. La configuración predeterminada usa autenticación de Windows y confianza del certificado para uso local.
