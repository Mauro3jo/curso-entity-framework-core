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

## Ejecutar

Se necesita el SDK .NET 10. Las pruebas con SQL Server usan Windows y LocalDB; cada ejercicio indica sus requisitos y comandos. Los paquetes se restauran desde nuget.org mediante la configuración de este repositorio.

Hay dos bases de ejemplo persistentes en la instancia local `CodexEfCoreCurso`: una para Database First y otra para el blog. Sus datos son ficticios. Las comprobaciones de relaciones usan bases temporales propias y las eliminan al terminar.

Los commits registran el trabajo de reconstrucción con sus fechas reales. Todavía faltan los bloques de consultas avanzadas, Fluent API, SQL y MySQL. El certificado se agregará al final, después de revisar los ejercicios.
