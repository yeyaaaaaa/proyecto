using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Proyecto.Pages
{
    [Authorize(Roles = "Paciente")]
    public class ResultadosModel : PageModel
    {
        private readonly ProyectoDbContext _context;

        public ResultadosModel(ProyectoDbContext context)
        {
            _context = context;
        }

        public List<ResultadoPacienteViewModel> Resultados { get; set; } = new();

        public async Task OnGetAsync()
        {
            var documento = User.FindFirstValue("Documento");
            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Usuario.Documento == documento);

            if (paciente == null)
            {
                Resultados = new List<ResultadoPacienteViewModel>();
                return;
            }

            var citasConResultados = await _context.Citas
                .Include(c => c.Examen)
                .Include(c => c.Resultado)
                .Where(c => c.PacienteID == paciente.PacienteID
                            && c.Resultado != null
                            && !string.IsNullOrEmpty(c.Resultado.ArchivoPDF)
                            && c.FechaHora <= DateTime.Now) // Solo citas pasadas
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            Resultados = citasConResultados.Select(c => new ResultadoPacienteViewModel
            {
                ExamenNombre = c.Examen.Nombre,
                FechaHora = c.FechaHora,
                RutaArchivo = c.Resultado.ArchivoPDF
            }).ToList();
        }

        public class ResultadoPacienteViewModel
        {
            public string ExamenNombre { get; set; }
            public DateTime FechaHora { get; set; }
            public string RutaArchivo { get; set; }
        }
    }
}