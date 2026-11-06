namespace Academia;

public enum NivelEducativo { Basico, Intermedio, Avanzado }
public class Direccion
{
    public string Calle { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
}
public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateTime FechaRegistro { get; set; }
    public Direccion Direccion { get; set; } = new();
    public PerfilAcademico? Perfil { get; set; }
    public List<Inscripcion> Inscripciones { get; set; } = [];
}
public class PerfilAcademico
{
    public int EstudianteId { get; set; }
    public string Carrera { get; set; } = string.Empty;
    public Estudiante Estudiante { get; set; } = null!;
}
public class Curso
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int DuracionHoras { get; set; }
    public bool Activo { get; set; }
    public NivelEducativo Nivel { get; set; }
    public decimal Costo { get; set; }
    public decimal CostoConImpuesto { get; private set; }
    public List<Clase> Clases { get; set; } = [];
    public List<Inscripcion> Inscripciones { get; set; } = [];
}
public class Clase
{
    public int Id { get; set; }
    public int CursoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public Curso Curso { get; set; } = null!;
}
public class Inscripcion
{
    public int EstudianteId { get; set; }
    public int CursoId { get; set; }
    public DateTime Fecha { get; set; }
    public Estudiante Estudiante { get; set; } = null!;
    public Curso Curso { get; set; } = null!;
}
