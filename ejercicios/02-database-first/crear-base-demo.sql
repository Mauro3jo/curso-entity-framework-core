USE [master];
GO

-- Detener con sqlcmd -b si la base ya existe. No se borra ni reemplaza.
IF DB_ID(N'EfCoreCursoDatabaseFirst02') IS NOT NULL
    THROW 50001, 'La base de practica ya existe; revise el destino antes de continuar.', 1;
GO

CREATE DATABASE [EfCoreCursoDatabaseFirst02];
GO

USE [EfCoreCursoDatabaseFirst02];
GO

CREATE TABLE dbo.categoria (
    idcategoria int IDENTITY(1,1) NOT NULL CONSTRAINT PK_categoria PRIMARY KEY,
    nombre nvarchar(100) NOT NULL,
    descripcion nvarchar(250) NULL,
    activo bit NOT NULL
);
GO

-- Datos ficticios creados para este ejercicio; no provienen del respaldo.
INSERT INTO dbo.categoria (nombre, descripcion, activo)
VALUES
    (N'Programacion', N'Categoria de demostracion para repasar EF Core.', 1),
    (N'Bases de datos', NULL, 1);
GO
