namespace proyecto.Models;
public class Notificacion
{
    public int NotificacionID { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; } = DateTime.Now;
    public bool Leido { get; set; } = false;

    public int UsuarioFK { get; set; }
    public Usuario? Usuario { get; set; }
}