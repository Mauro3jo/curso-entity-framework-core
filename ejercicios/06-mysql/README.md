# 06. EF Core con MySQL

Los fragmentos `LibrosController.txt` e `Index.txt` del curso usan libros con título, autor, año, precio y disponibilidad. Este ejemplo reconstruye ese modelo y separa la prueba del proveedor de la interfaz MVC, que está en el blog.

```powershell
dotnet run --project ejercicios/06-mysql/LibrosMySql.csproj
```

Sin argumentos genera y comprueba el esquema MySQL sin abrir una conexión. Se usa `MySql.EntityFrameworkCore` 10.0.12 de Oracle con EF Core 10.0.14. El precio se mapea como `decimal(10,2)`.

## Prueba contra MySQL

Definir `EFCORE_MYSQL_CONNECTION` en el entorno local, con una conexión a un servidor de pruebas MySQL 8 o posterior y un usuario autorizado para crear y eliminar bases temporales. No guardar esa conexión ni sus credenciales en el repositorio.

```powershell
dotnet run --project ejercicios/06-mysql/LibrosMySql.csproj -- --verificar
```

La prueba reemplaza el nombre de base de la conexión por uno aleatorio `efcore_curso_checks_...`; crea esa base, verifica el CRUD y la precisión decimal, y elimina únicamente la base que acaba de crear. No utiliza el catálogo original de la conexión. Rechaza MariaDB porque este ejemplo utiliza el proveedor Oracle para MySQL.

## Estado

Compilación comprobada sin errores ni advertencias y esquema generado con los seis campos de `Libro`, incluida la precisión decimal. La prueba contra un servidor MySQL está pendiente. No se da por probado el CRUD real hasta ejecutarla.

Referencias: [paquete del proveedor](https://www.nuget.org/packages/MySql.EntityFrameworkCore/10.0.12) y [compatibilidad de Connector/NET](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html).
