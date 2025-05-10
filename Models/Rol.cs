namespace proyecto.Models;

public class Rol
{
    public int RolID { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Usuario>? Usuarios { get; set; }
}
