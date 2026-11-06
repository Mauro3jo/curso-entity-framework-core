# 06. MySQL

Modelo de libros armado a partir de `LibrosController.txt` e `Index.txt` del curso: título, autor, año, precio y disponibilidad. Es un ejemplo de consola con el proveedor Oracle `MySql.EntityFrameworkCore`.

```powershell
dotnet run --project ejercicios/06-mysql/LibrosMySql.csproj
```

Genera el esquema sin conectarse. Se comprobaron la compilación y el tipo `decimal(10,2)` del esquema. La prueba de CRUD contra MySQL real quedó pendiente.

Para ejecutarla, definir `EFCORE_MYSQL_CONNECTION` en el entorno local y usar:

```powershell
dotnet run --project ejercicios/06-mysql/LibrosMySql.csproj -- --verificar
```

Necesita MySQL 8 o posterior y permiso para crear y eliminar bases. La prueba usa un nombre aleatorio y borra su propia base al terminar; no usa el catálogo indicado en la conexión. El programa rechaza MariaDB. Las credenciales quedan fuera del repositorio.
