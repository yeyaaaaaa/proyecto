namespace proyecto.Models;

public class Paciente
{
    public int PacienteID { get; set; }
    public DateTime Nacimiento { get; set; }
    public string Sexo { get; set; } = "otro";
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int UsuarioFK { get; set; }

    public Usuario? Usuario { get; set; }
}