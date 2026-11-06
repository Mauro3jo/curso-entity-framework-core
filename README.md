# Entity Framework Core con .NET 10

Ejercicios del curso **Máster en Entity Framework Core con .NET 10 – Curso Completo**, de [render2web en Udemy](https://ucic.udemy.com/course/curso-entity-framework-core/).

El curso está terminado: Udemy muestra 168 de 168 elementos completados. Los archivos de práctica originales se perdieron; estos ejemplos se reconstruyeron a partir de los temas y materiales del curso. Las descargas del profesor se usan como referencia.

## Ejercicios

| Tema | Código e instrucciones |
| --- | --- |
| Modelos y migraciones | [01. Code First](ejercicios/01-code-first/README.md) |
| Cambios de esquema | [Evolución del modelo](ejercicios/01-code-first/evolucion-del-modelo.md) |
| Ingeniería inversa | [02. Database First](ejercicios/02-database-first/README.md) |
| Validaciones | [03. Data Annotations](ejercicios/03-validaciones/README.md) |
| Relaciones y CRUD MVC | [04. Blog](ejercicios/04-blog/README.md) |
| LINQ y cambios masivos | [Consultas del blog](ejercicios/04-blog/consultas.md) |
| Configuración del modelo | [05. Fluent API](ejercicios/05-fluent-api/README.md) |
| Vistas y procedimientos | [SQL del modelo académico](ejercicios/05-fluent-api/sql.md) |
| Proveedor MySQL | [06. Libros](ejercicios/06-mysql/README.md) |

Cada carpeta concentra un tema. El CRUD web está implementado para artículos; el ejemplo de libros usa una consola. No se intentó reproducir cada pantalla ni cada controlador del material descargado.

## Ejecutar

Se necesita el SDK .NET 10. Las pruebas con SQL Server usan Windows y la instancia LocalDB `CodexEfCoreCurso`, cuya preparación está en Database First. Los paquetes se restauran desde nuget.org mediante la configuración de este repositorio.

```powershell
.\comprobar-ejercicios.ps1
```

El script compila los seis proyectos y ejecuta las comprobaciones de consola y SQL Server. Las pruebas HTTP del blog y el CRUD contra MySQL tienen comandos separados en sus carpetas.

## Estado de las pruebas

- Compilación de los seis proyectos sin errores ni advertencias.
- Validaciones, relaciones, consultas, restricciones de Fluent API y procedimientos comprobados.
- CRUD MVC probado por HTTP, incluidos formularios inválidos y conflictos de concurrencia.
- MySQL: esquema generado y revisado; la prueba contra un servidor real sigue pendiente.

Hay dos bases de ejemplo persistentes en la instancia local del curso: una para Database First y otra para el blog. Sus datos son ficticios. Las demás pruebas SQL Server crean bases temporales propias y las eliminan al terminar. El respaldo original del curso no está publicado.

Los commits registran el trabajo de reconstrucción con sus fechas reales. El certificado está revisado y se agregará al último, después de cerrar las pruebas pendientes.
