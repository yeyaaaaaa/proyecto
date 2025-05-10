namespace proyecto.Models;

public class HistorialCita
{
    public int HistorialCitaID { get; set; }
    public string Accion { get; set; } = string.Empty;
    public DateTime FechaAccion { get; set; } = DateTime.Now;

    public int CitaFK { get; set; }
    public Cita? Cita { get; set; }

    public int UsuarioAccion { get; set; }
    public Usuario? Usuario { get; set; }
}