namespace proyecto.Models;

public class LogSistema
{
    public int LogSistemaID { get; set; }
    public string Accion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;

    public int UsuarioFK { get; set; }
    public Usuario? Usuario { get; set; }
}