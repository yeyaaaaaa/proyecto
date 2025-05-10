namespace proyecto.Models;

public class EstadoCita
{
    public int EstadoCitaID { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Cita>? Citas { get; set; }
}