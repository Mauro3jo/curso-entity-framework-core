# 03. Data Annotations

Validaciones de artículos y usuarios con `Required`, `MaxLength`, `Range`, `EmailAddress` y un atributo `NoEspacios`.

```powershell
dotnet run --project ejercicios/03-validaciones/Validaciones.csproj
```

La consola comprueba 12 casos: entradas válidas, límites de longitud y tiempo, correo inválido y alias con espacios o tabuladores. Falla si una regla devuelve un resultado inesperado.

`Required` controla los valores vacíos. `NoEspacios` se aplica al alias y rechaza caracteres de espacio, incluidos tabuladores; no se aplica al nombre de una persona. `Display` cambia la etiqueta del campo, no su validez.

El ejemplo reconstruye los temas de `DataAnnotationsEFCore.rar` con clases nuevas y datos ficticios. Usa `Validator.TryValidateObject` y no necesita una base de datos. Pasaron los 12 casos y la compilación quedó sin errores ni advertencias.
