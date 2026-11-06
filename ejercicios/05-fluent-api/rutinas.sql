CREATE OR ALTER VIEW Academico.vw_Inscripciones
AS
SELECT c.Id AS CursoId, c.Titulo AS Curso, c.DuracionHoras, c.Activo,
       COUNT(i.EstudianteId) AS TotalInscritos, AVG(CAST(e.Edad AS float)) AS EdadPromedio
FROM Academico.Cursos AS c
LEFT JOIN Academico.Inscripciones AS i ON i.CursoId = c.Id
LEFT JOIN Academico.Estudiantes AS e ON e.Id = i.EstudianteId
GROUP BY c.Id, c.Titulo, c.DuracionHoras, c.Activo;
GO

CREATE OR ALTER PROCEDURE Academico.CursosPorDuracion @MinimoHoras int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CursoId, Curso, DuracionHoras, Activo, TotalInscritos, EdadPromedio
    FROM Academico.vw_Inscripciones
    WHERE Activo = 1 AND DuracionHoras >= @MinimoHoras;
END;
GO

CREATE OR ALTER PROCEDURE Academico.InsertarEstudiante
    @Nombre nvarchar(100), @Email varchar(150), @Edad int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Academico.Estudiantes (Nombre, Email, Edad, DireccionCalle, DireccionCiudad)
    VALUES (@Nombre, @Email, @Edad, N'', N'');
END;
GO

CREATE OR ALTER PROCEDURE Academico.ActualizarEdad @Id int, @Edad int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Academico.Estudiantes SET Edad = @Edad WHERE Id = @Id;
END;
GO
