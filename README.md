# Entity Framework Core con .NET 10

Repositorio de aprendizaje de Mauro Trejo para organizar, por etapas, los ejercicios y proyectos relacionados con el curso **Máster en Entity Framework Core con .NET 10 – Curso Completo**, de render2web.

[Ver el curso en Udemy](https://ucic.udemy.com/course/curso-entity-framework-core/).

## Curso completado

La cuenta de Udemy muestra **168 de 168 elementos completados** y el certificado disponible. Este estado se verificó directamente en el aula del curso.

## Temas del recorrido

- Modelos, contexto y migraciones con Code First.
- Ingeniería inversa con Database First.
- Validaciones con Data Annotations y configuración con Fluent API.
- Relaciones entre entidades, consultas LINQ y operaciones CRUD.
- Vistas y procedimientos almacenados de SQL Server.
- Integración de Entity Framework Core con MySQL.

## Estado del repositorio

Los ejercicios se están reconstruyendo a partir de los temas y materiales del curso porque los archivos de práctica originales se perdieron. El código de cada reconstrucción se escribe de nuevo y se verifica ahora; no se presenta como una recuperación de aquellos archivos ni como trabajo realizado en fechas anteriores.

El primer ejercicio es [Code First: modelos, contexto y migración](ejercicios/01-code-first/README.md). Incluye `Categoria`, `Usuario`, un contexto EF Core y la generación del esquema SQL Server. Sus instrucciones distinguen las comprobaciones sin conexión de la creación opcional de una base local.

La siguiente práctica del mismo ejercicio muestra la [evolución del modelo y una migración incremental](ejercicios/01-code-first/evolucion-del-modelo.md): agregar una columna opcional y revisar qué ocurriría al revertirla.

El segundo ejercicio, [Database First e ingeniería inversa](ejercicios/02-database-first/README.md), parte de una base nueva con dos categorías ficticias y genera el modelo desde SQL Server. Incluye el script para reproducir esa base de práctica sin utilizar el respaldo original.

Los proyectos descargados del instructor se conservan aparte como referencia. El certificado y el respaldo de base de datos todavía no se han publicado.

## Próximas etapas

1. Continuar con el siguiente tema del curso en un ejercicio separado.
2. Añadir el certificado y una base de demostración revisada en cambios separados.

El historial reflejará avances reales con fechas actuales. Este repositorio corresponde exclusivamente a este curso; los otros cursos se organizarán por separado.
