# 04. Blog: relaciones entre entidades

Reconstrucción basada en los nombres de entidades y temas de `RelacionesConEFCore.rar`: usuarios, perfiles, categorías, artículos, comentarios y etiquetas. El código y los datos de demostración se escribieron de nuevo.

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --verificar
```

Requiere la instancia `(localdb)\CodexEfCoreCurso` preparada en Database First. La comprobación crea una base con nombre aleatorio `EfCoreCursoChecks_...`, verifica relaciones 1:1, 1:N y N:N, borrado restringido y cascadas, y elimina exclusivamente esa base temporal al terminar.

La categoría con artículos no se puede borrar; borrar un artículo elimina sus comentarios y enlaces, pero conserva las etiquetas. Borrar un usuario sin artículos ni comentarios elimina su perfil. Las relaciones se configuran explícitamente para evitar rutas de cascada ambiguas.

Para preparar una base persistente de datos ficticios:

```powershell
dotnet run --project ejercicios/04-blog/Blog.csproj -- --crear-demo
```

Crea `EfCoreCursoBlog04` únicamente si no existe. No reemplaza una base anterior. El arranque web normal no crea ni modifica el esquema.

Este bloque usa `EnsureCreated` para una demostración aislada; no mezcla ese flujo con las migraciones del primer ejercicio ni lo presenta como despliegue de producción. `Version` prepara el control de concurrencia con `rowversion`. El siguiente bloque añadirá controladores y vistas a este mismo proyecto.
