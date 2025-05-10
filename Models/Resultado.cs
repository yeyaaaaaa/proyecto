namespace proyecto.Models;

public class Resultado
{
    public int ResultadoID { get; set; }
    public string ArchivoPDF { get; set; } = string.Empty;
    public DateTime FechaSubida { get; set; }

    public int CitaFK { get; set; }
    public Cita? Cita { get; set; }

    public int SubidoPor { get; set; }
    public Usuario? Usuario { get; set; }
}