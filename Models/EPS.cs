namespace proyecto.Models;

public class EPS
{
    public int EPSID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string NIT { get; set; } = string.Empty;

    public ICollection<Afiliacion>? Afiliaciones { get; set; }
}