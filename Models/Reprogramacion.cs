namespace proyecto.Models;

public class Reprogramacion
{
    public int ReprogramacionID { get; set; }
    public DateTime NuevaFecha { get; set; }
    public TimeSpan NuevaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;

    public int CitaFK { get; set; }
    public Cita? Cita { get; set; }
}