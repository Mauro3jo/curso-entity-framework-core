BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261106013717_AgregarDescripcionCategoria'
)
BEGIN
    ALTER TABLE [Categorias] ADD [Descripcion] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261106013717_AgregarDescripcionCategoria'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261106013717_AgregarDescripcionCategoria', N'10.0.14');
END;

COMMIT;
GO

