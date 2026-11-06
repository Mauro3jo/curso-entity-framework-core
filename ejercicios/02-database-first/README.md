# 02. Database First: generar el modelo desde SQL Server

Reconstrucción de una práctica de ingeniería inversa del curso. El recurso local `CursoEFCoreDatabaseFirst.rar` muestra una entidad `categoria` con nombre, descripción opcional y estado activo. Aquí se construyó una base nueva con esa forma mínima para practicar el proceso, sin restaurar ni redistribuir el respaldo original.

## El recorrido

1. Crear en SQL Server la tabla `categoria` y dos registros ficticios.
2. Ejecutar `dotnet ef dbcontext scaffold` contra esa base.
3. Compilar las clases generadas y consultar las categorías mediante EF Core.

La fuente del modelo es la base, a diferencia de Code First. No se agregan migraciones a este proyecto: modificar la base y regenerar sus clases es un flujo distinto que debe revisarse antes de sobrescribir archivos.

## Ver el esquema sin instalar SQL Server

Con SDK .NET 10, desde la raíz:

```powershell
dotnet tool restore
dotnet restore ejercicios/02-database-first/DatabaseFirst.csproj
dotnet build ejercicios/02-database-first/DatabaseFirst.csproj --no-restore
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj --no-build
```

La ejecución sin argumentos imprime el SQL del modelo generado sin conectarse a una base. La configuración NuGet del repositorio utiliza únicamente la fuente pública oficial y no cambia la configuración del equipo.

## Preparar la base ficticia en Windows

Requisitos adicionales: SQL Server LocalDB y `sqlcmd`. Estos pasos crean una instancia separada y una base nueva; no son necesarios para imprimir el esquema.

```powershell
SqlLocalDB create CodexEfCoreCurso 17.0
SqlLocalDB start CodexEfCoreCurso
sqlcmd -S "(localdb)\CodexEfCoreCurso" -E -b -i ejercicios/02-database-first/crear-base-demo.sql
```

Si la instancia ya existe, se omite su creación. El script espera que la base `EfCoreCursoDatabaseFirst02` no exista; con `-b`, se detiene con error si encuentra ese nombre y no la borra ni la reemplaza. La base recién creada contiene solo dos categorías ficticias. El script no incluye personas, correos, contraseñas ni datos extraídos del respaldo.

Para consultar esas categorías:

```powershell
dotnet run --project ejercicios/02-database-first/DatabaseFirst.csproj --no-build -- --consultar
```

La salida esperada contiene `Programacion` y `Bases de datos`. Este modo sí se conecta a la base y ejecuta una consulta de lectura.

## Repetir el scaffolding

Comando usado para generar `Models/Categoria.cs` y `Datos/CategoriasDbContext.cs`:

```powershell
dotnet ef dbcontext scaffold "Server=(localdb)\CodexEfCoreCurso;Database=EfCoreCursoDatabaseFirst02;Integrated Security=True;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project ejercicios/02-database-first --context CategoriasDbContext --context-dir Datos --output-dir Models --namespace DatabaseFirst.Models --context-namespace DatabaseFirst.Datos --data-annotations --no-onconfiguring --no-pluralize --table dbo.categoria
```

Los archivos ya están incluidos. Repetirlo en el mismo directorio requiere revisar las diferencias y autorizar la sustitución de los generados; no se agrega `--force` al comando de uso habitual para evitar sobrescribir modificaciones por accidente.

`--no-onconfiguring` evita incrustar la conexión en el contexto generado; `Program.cs` la proporciona mediante opciones. `--data-annotations` genera atributos del esquema y `--no-pluralize` evita que la singularización automática transforme `Categoria` en `Categorium`. El filtro de tabla limita lo que se importa.

La conexión predeterminada usa autenticación integrada de Windows y una instancia local. `TrustServerCertificate=True` se limita a esa práctica local. Otra conexión puede configurarse con la variable de entorno `EFCORE_DATABASEFIRST_SQLSERVER`, sin escribir credenciales en el repositorio.

## Verificación realizada

- Base creada en una instancia LocalDB 17.0 separada para el curso: una tabla y dos registros ficticios.
- Clases generadas mediante ingeniería inversa de esa base, sin conexión incrustada en el contexto.
- Compilación con SDK 10.0.403: cero errores y cero advertencias.
- Ejecución sin conexión: SQL con los cuatro campos esperados de `categoria`.
- Consulta real de lectura: devuelve las dos categorías ficticias en orden.
- Repetición del script con `sqlcmd -b`: se detiene porque la base existe y conserva sus dos registros.
- Una opción desconocida de la consola termina con código 1 antes de abrir una conexión.

Estas comprobaciones corresponden exclusivamente a la nueva base de demostración. No verifican ni restauran el respaldo original del curso.

## Alcance y referencias

- Se reconstruyó únicamente la tabla necesaria para esta primera práctica de Database First, no la base completa del curso.
- Las clases se generaron realmente desde la base ficticia con EF Core 10.0.14. No se copiaron del proyecto del instructor.
- No hay tablas de usuarios, datos personales ni autenticación en este ejercicio.
- Se conserva la corrección de la dependencia transitiva `System.Security.Cryptography.Pkcs` usada en el ejercicio anterior.
- [Curso de referencia](https://ucic.udemy.com/course/curso-entity-framework-core/).
- [Documentación oficial de ingeniería inversa](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/).
