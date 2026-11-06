# 01. Code First

Modelos `Categoria` y `Usuario`, contexto y migraciones. La primera crea las tablas; la segunda agrega una descripción opcional a la categoría.

Desde la raíz:

```powershell
dotnet tool restore
dotnet run --project ejercicios/01-code-first/CodeFirst.csproj
dotnet ef migrations has-pending-model-changes --project ejercicios/01-code-first
```

La consola imprime SQL sin conectarse a una base. [Detalle de la segunda migración](evolucion-del-modelo.md).

Para aplicar las migraciones en LocalDB, opcional:

```powershell
dotnet ef database update --project ejercicios/01-code-first
```

Ese comando crea o actualiza `EfCoreAprendizaje01` en `(localdb)\MSSQLLocalDB`, con autenticación de Windows. Se puede cambiar la conexión mediante `EFCORE_APRENDIZAJE_SQLSERVER`. La confianza del certificado está configurada para la práctica local.

Se revisaron el modelo y el SQL generado; no se aplicaron estas migraciones sobre una base con registros.
