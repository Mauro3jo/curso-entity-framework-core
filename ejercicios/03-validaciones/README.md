# 03. Data Annotations y validación

Reconstrucción del bloque de validaciones. Los materiales `DataAnnotationsEFCore.rar` muestran artículos, usuarios, límites de longitud, rangos, correos y un atributo personalizado `NoEspaciosAttribute`. Esta práctica vuelve a implementar esos conceptos con modelos pequeños y datos ficticios.

```powershell
dotnet run --project ejercicios/03-validaciones/Validaciones.csproj
```

La consola realiza 12 comprobaciones: casos válidos, límites inclusivos de longitud y tiempo, valores fuera de rango, correo inválido y alias con espacios o tabuladores. Cualquier resultado inesperado termina con error.

`Required` maneja valores vacíos; `NoEspacios` rechaza caracteres de espacio Unicode, incluidos tabuladores. Esta regla se aplica a un alias, no al nombre de una persona. Es una decisión explícita de esta reconstrucción; no pretende recuperar exactamente el atributo original.

Se usan `Required`, `MaxLength`, `Range`, `EmailAddress` y `Display`. La validación se ejecuta mediante `Validator.TryValidateObject`; no conecta a una base de datos. Los atributos de presentación no validan por sí solos y los atributos de esquema de EF Core son un tema distinto, abordado en los ejercicios de persistencia.

Comprobación realizada: 12 casos correctos y compilación sin errores ni advertencias con SDK .NET 10. Los casos se escribieron antes de implementar los modelos y se comprobó que inicialmente faltaban los tipos necesarios.
