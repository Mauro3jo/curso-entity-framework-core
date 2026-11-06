# EF Core con .NET 10

El proyecto principal es un blog en C# con ASP.NET Core MVC y SQL Server LocalDB. Tiene CRUD de artículos, búsqueda, paginación y control de concurrencia. El resto de las carpetas contiene ejemplos de EF Core.

## Ejecutar el blog

Requiere Windows, SDK .NET 10 y LocalDB 17. Desde la raíz:

```powershell
SqlLocalDB create CodexEfCoreCurso 17.0
SqlLocalDB start CodexEfCoreCurso
dotnet run --project ejercicios/04-blog/Blog.csproj -- --crear-demo
dotnet run --project ejercicios/04-blog/Blog.csproj --urls http://127.0.0.1:5087
```

Abrir `http://127.0.0.1:5087/Articulos`. Omitir la creación de la instancia y de la base si ya existen. La web usa datos ficticios y no tiene inicio de sesión.

## Ejemplos

- [Code First y migraciones](ejercicios/01-code-first/README.md)
- [Database First](ejercicios/02-database-first/README.md)
- [Data Annotations](ejercicios/03-validaciones/README.md)
- [Blog: relaciones y consultas](ejercicios/04-blog/README.md)
- [Fluent API y procedimientos SQL](ejercicios/05-fluent-api/README.md)
- [Libros con MySQL](ejercicios/06-mysql/README.md)

`comprobar-ejercicios.ps1` compila los seis proyectos y ejecuta las pruebas de consola y SQL Server. Las pruebas HTTP están en la carpeta del blog. En MySQL están comprobados la compilación y el esquema; el CRUD contra un servidor real sigue pendiente.

Referencia: [curso de EF Core de render2web](https://ucic.udemy.com/course/curso-entity-framework-core/). Los ejemplos están rehechos a partir de ese material porque se perdieron los archivos originales. [Certificado](certificados/certificado-entity-framework-core.pdf).
