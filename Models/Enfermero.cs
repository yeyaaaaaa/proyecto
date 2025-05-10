namespace proyecto.Models;
public class Enfermero
{
    public int EnfermeroID { get; set; }
    public string Licencia { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public int UsuarioFK { get; set; } 

    public Usuario? Usuario { get; set; }
    public ICollection<Cita>? Citas { get; set; }
}