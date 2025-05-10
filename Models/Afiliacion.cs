namespace proyecto.Models;

public class Afiliacion
{
    public int AfiliacionID { get; set; }
    public string TipoIdentificacion { get; set; } = string.Empty;

    public int UsuarioFK { get; set; }
    public Usuario? Usuario { get; set; }

    public int EPSFK { get; set; }
    public EPS? EPS { get; set; }
}
