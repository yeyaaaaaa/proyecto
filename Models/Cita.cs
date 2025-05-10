namespace proyecto.Models;

public class Cita
{
    public int CitaID { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }

    public int UsuarioFK { get; set; }
    public Usuario? Usuario { get; set; }

    public int EnfermeroFK { get; set; }
    public Enfermero? Enfermero { get; set; }

    public int ExamenFK { get; set; }
    public Examen? Examen { get; set; }

    public int EstadoFK { get; set; }
    public EstadoCita? Estado { get; set; }

    public Resultado? Resultado { get; set; }
    public ICollection<HistorialCita>? Historial { get; set; }
    public ICollection<Reprogramacion>? Reprogramaciones { get; set; }
}