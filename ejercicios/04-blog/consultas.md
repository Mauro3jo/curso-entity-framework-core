# Consultas y seguimiento de cambios

Los controladores del material del curso usan carga de relaciones, proyecciones y consultas LINQ. `Consultas.cs` reúne ejemplos pequeños que se pueden ejecutar sobre datos conocidos.

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --consultas
```

Se crea una base temporal propia con tres artículos ficticios y se elimina al terminar. Se comprueban:

- `IQueryable`, ejecución diferida y SQL generado con `ToQueryString`.
- Filtros, proyecciones, orden estable y paginación con `Skip` y `Take`.
- Agrupación y promedio con `GroupBy`, combinación con `Join` y colecciones con `SelectMany`.
- `Include` filtrado y consultas sin seguimiento.
- `Attach` para persistir únicamente un campo de una entidad desconectada.
- `ExecuteUpdate` y `ExecuteDelete` dentro de una transacción que se revierte.

Las operaciones masivas se ejecutan inmediatamente y no sincronizan el change tracker ni aplican automáticamente el control de concurrencia. Por eso este ejemplo limpia el seguimiento antes de ejecutarlas y comprueba las filas afectadas. No recibe entidades ni filtros de una petición web.

El CRUD MVC usa formularios limitados y `rowversion`; no debe sustituirse por un `Update` de una entidad recibida del cliente. Para procesar datos en memoria se materializa primero la consulta: hacerlo antes de filtrar puede cargar más filas de las necesarias.

Referencia: [ExecuteUpdate y ExecuteDelete](https://learn.microsoft.com/en-us/ef/core/saving/execute-insert-update-delete).
