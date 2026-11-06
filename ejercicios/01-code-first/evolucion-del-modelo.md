# Evolución del modelo sin recrear las tablas

Esta segunda práctica amplía la reconstrucción de Code First: `Categoria` incorpora `Descripcion` como `string?`. El signo `?` indica que la propiedad admite `null`; EF Core la representa como una columna opcional en SQL Server.

## Qué cambia

La migración `AgregarDescripcionCategoria` agrega únicamente `Descripcion` a `Categorias`, como `nvarchar(max) NULL`. No recrea la tabla ni cambia `Usuarios`. Los registros que existan no necesitan una descripción: el nuevo campo puede quedar en `NULL`.

Las dos migraciones muestran el recorrido:

1. `Inicial`: tablas `Categorias` y `Usuarios`.
2. `AgregarDescripcionCategoria`: columna opcional en `Categorias`.

El snapshot representa el modelo más reciente, mientras las migraciones anteriores conservan el historial del esquema.

## Generar y revisar el cambio

Desde la raíz del repositorio, después de restaurar dependencias y herramientas:

```powershell
dotnet build ejercicios/01-code-first/CodeFirst.csproj --no-restore
dotnet ef migrations has-pending-model-changes --project ejercicios/01-code-first --no-build
dotnet ef migrations script Inicial AgregarDescripcionCategoria --idempotent --project ejercicios/01-code-first --no-build --output ejercicios/01-code-first/evolucion-categoria.sql
```

El script versionado `evolucion-categoria.sql` incluye comprobaciones del historial de EF para omitir la migración si ya fue aplicada. Espera una base que tenga la migración `Inicial`; no sustituye al esquema inicial ni comprueba por sí mismo que la estructura no se haya modificado manualmente.

Estos comandos compilan y generan SQL. No aplican la migración, no abren una conexión y no modifican una base de datos. Al ejecutar la consola, el SQL del esquema completo ya incluye la descripción opcional.

## Entender la reversión

Se puede generar el SQL inverso sin ejecutarlo:

```powershell
dotnet ef migrations script AgregarDescripcionCategoria Inicial --project ejercicios/01-code-first --no-build
```

La reversión elimina la columna `Descripcion`. Si se hubiera utilizado una base real y guardado descripciones, esos valores se perderían al aplicarla. No se ejecutó esa reversión ni una actualización de base como parte de esta práctica.

El avance permite que las categorías anteriores sigan sin descripción. Una versión anterior de la aplicación puede seguir usando sus columnas originales mientras la nueva columna permanece opcional. No se está demostrando una actualización entre aplicaciones desplegadas.

## Alcance de la comprobación

La comprobación de este ejercicio cubre compilación, coherencia del modelo con la migración y revisión del SQL de avance y reversión. No acredita una migración sobre datos existentes: no se restauró ni se modificó ninguna base.

Se mantiene el carácter de reconstrucción educativa del curso; este cambio no recupera los archivos de práctica originales.
