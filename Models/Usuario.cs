namespace proyecto.Models;

public class Usuario
{

    public required int UsuarioID { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Correo { get; set; }
    public required string Contraseña { get; set; }
    public required string TipoIdentificacion { get; set; } = string.Empty;
    public required int Documento { get; set; }
    public bool Estado { get; set; } = true;

    public required int RolFK { get; set; }
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

    public void SetContraseña(string contraseña)
    {
        if (string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("La contraseña no puede estar vacía.");

        if (contraseña.Length < 8)
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");

        if (!contraseña.Any(char.IsDigit))
            throw new ArgumentException("La contraseña debe contener al menos un número.");

        if (!contraseña.Any(char.IsLetter))
            throw new ArgumentException("La contraseña debe contener al menos una letra.");

        Contraseña = contraseña;
    }
}