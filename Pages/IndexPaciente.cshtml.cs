using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Proyecto.Data;
using Proyecto.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Proyecto.Pages
{
    [Authorize(Roles = "Paciente")]
    public class IndexPacienteModel : PageModel
    {
        private readonly ProyectoDbContext _context;
        public string NombrePaciente { get; set; } = "";

        public List<Cita> Citas { get; set; } = new List<Cita>();

        public IndexPacienteModel(ProyectoDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var documento = User.FindFirstValue("Documento");
            var paciente = _context.Pacientes.Include(p => p.Usuario).FirstOrDefault(p => p.Usuario.Documento == documento);

            if (paciente != null)
            {
                NombrePaciente = paciente.Nombres;
                // Trae solo citas activas y futuras
                Citas = _context.Citas
                    .Include(c => c.Examen)
                    .Where(c => c.PacienteID == paciente.PacienteID && c.Estado == EstadoGeneral.Activo && c.FechaHora >= System.DateTime.Now)
                    .OrderBy(c => c.FechaHora)
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPostCancelarCitaAsync(int citaId)
        {
            var cita = await _context.Citas.FindAsync(citaId);
            if (cita != null && cita.Estado == EstadoGeneral.Activo)
            {
                cita.Estado = EstadoGeneral.Inactivo; // O el estado adecuado para "cancelada"
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}