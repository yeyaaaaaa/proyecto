using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Proyecto.Data;
using Proyecto.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var paciente = _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.Usuario.Documento == documento);

            if (paciente != null)
            {
                NombrePaciente = paciente.Nombres;
                // Trae solo citas activas y futuras, incluye resultados
                Citas = _context.Citas
                    .Include(c => c.Examen)
                    .Include(c => c.Resultado)
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
                TempData["MensajeExito"] = "La cita fue cancelada correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cancelar la cita.";
            }
            return RedirectToPage();
        }

        // Handler para mostrar detalle de resultado en el modal (AJAX)
        public async Task<IActionResult> OnGetDetalleResultadoAsync(int citaId)
        {
            var cita = await _context.Citas
                .Include(c => c.Examen)
                .Include(c => c.Resultado)
                .FirstOrDefaultAsync(c => c.CitaID == citaId);

            if (cita == null)
                return NotFound();

            var detalle = new
            {
                examenNombre = cita.Examen?.Nombre,
                fechaHora = cita.FechaHora,
                estado = cita.Estado.ToString(),
                resultadoArchivo = cita.Resultado?.ArchivoPDF
            };

            return new JsonResult(detalle);
        }
    }
}