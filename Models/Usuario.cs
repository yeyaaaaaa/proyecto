namespace proyecto.Models;

public class Usuario
{
    public int UsuarioID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contraseña { get; set; } = string.Empty;
    public int Documento { get; set; }
    public bool Estado { get; set; } = true;

    public int RolFK { get; set; }
    public Rol? Rol { get; set; }

    public Paciente? Paciente { get; set; }
    public Enfermero? Enfermero { get; set; }
    public Afiliacion? Afiliacion { get; set; }
    public ICollection<Afiliacion>? Afiliaciones { get; set; }
    public ICollection<Cita>? Citas { get; set; }
    public ICollection<Resultado>? ResultadosSubidos { get; set; }
    public ICollection<Notificacion>? Notificaciones { get; set; }
    public ICollection<HistorialCita>? HistorialCitas { get; set; }
    public ICollection<LogSistema>? Logs { get; set; }
    
}