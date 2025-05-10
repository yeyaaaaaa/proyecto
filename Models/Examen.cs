namespace proyecto.Models;

public class Examen
{
    public int ExamenID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public ICollection<Cita>? Citas { get; set; }
}