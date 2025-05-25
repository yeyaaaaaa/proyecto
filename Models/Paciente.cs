using System.ComponentModel.DataAnnotations;
namespace proyecto.Models;

public class Paciente
{
    public required int PacienteID { get; set; }
    public required DateTime Nacimiento { get; set; }
    public required string Sexo { get; set; }
    public required string Direccion { get; set; }
    public required string Telefono { get; set; }
    public required int UsuarioFK { get; set; }

    public Usuario? Usuario { get; set; }
}