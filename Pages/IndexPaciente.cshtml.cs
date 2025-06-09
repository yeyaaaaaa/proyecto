using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Proyecto.Data;

namespace Proyecto.Pages
{
    [Authorize]
    public class IndexPacienteModel : PageModel
    {
        private readonly ProyectoDbContext _context;
        public string NombrePaciente { get; set; } = "";

        public IndexPacienteModel(ProyectoDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Se guarda el documento como claim
            var documento = User.FindFirstValue("Documento");

            // Busca el paciente en la base de datos usando el documento
            var paciente = _context.Pacientes.FirstOrDefault(p => p.Usuario.Documento == documento);

            if (paciente != null)
                NombrePaciente = paciente.Nombres; 
        }
    }
}