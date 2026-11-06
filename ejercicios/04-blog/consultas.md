# Consultas LINQ

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --consultas
```

`Consultas.cs` crea tres artículos ficticios en una base temporal y comprueba filtros, proyecciones, paginación, `GroupBy`, `Join`, `SelectMany`, `Include` filtrado y `Attach`.

También prueba `ExecuteUpdate` y `ExecuteDelete` dentro de una transacción que se revierte. Estas operaciones se ejecutan directamente en la base: no sincronizan el change tracker ni usan automáticamente el control de concurrencia. El ejemplo limpia el seguimiento y comprueba las filas afectadas.

La base temporal se elimina al terminar. El CRUD web usa formularios con campos permitidos y `rowversion`, no entidades recibidas directamente del cliente.
