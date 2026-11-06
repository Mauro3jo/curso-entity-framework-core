# 04. Blog

Usuarios, perfiles, categorías, artículos, comentarios y etiquetas. El ejemplo tiene relaciones 1:1, 1:N y N:N, y CRUD MVC de artículos con búsqueda y paginación.

Necesita la instancia LocalDB preparada en Database First. Crear la base de ejemplo una sola vez y arrancar la web:

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --crear-demo
dotnet run --project ejercicios/04-blog/Blog.csproj --urls http://127.0.0.1:5087
```

Abrir `http://127.0.0.1:5087/Articulos`. `--crear-demo` se detiene si `EfCoreCursoBlog04` ya existe. La web no crea el esquema al arrancar.

Los formularios validan los campos y usan token antifalsificación. Una edición o un borrado con `rowversion` desactualizado devuelve HTTP 409. Es una práctica local sin inicio de sesión.

Para probar relaciones y consultas:

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --verificar
dotnet run --project ejercicios/04-blog/Blog.csproj -- --consultas
```

Estas pruebas crean bases temporales propias y las eliminan al terminar. Una categoría con artículos no se puede borrar; borrar un artículo elimina sus comentarios y enlaces, pero conserva las etiquetas.

Con la web encendida, las pruebas HTTP requieren Python 3:

```powershell
python ejercicios/04-blog/comprobar-web.py
```

Crean y borran un artículo ficticio y comprueban el CRUD, validaciones, antifalsificación y concurrencia. [Ejemplos de consultas LINQ](consultas.md).

Esta base usa `EnsureCreated`; las migraciones están en el primer ejercicio.
