# 03. Data Annotations

Artículos y usuarios con campos obligatorios, longitudes máximas, rangos y correo válido.

```powershell
dotnet run --project ejercicios/03-validaciones/Validaciones.csproj
```

Ejecuta 12 casos con `Validator.TryValidateObject`; no necesita una base de datos. El atributo `NoEspacios` se aplica al alias y rechaza espacios y tabuladores. `Display` solo cambia la etiqueta del campo.
