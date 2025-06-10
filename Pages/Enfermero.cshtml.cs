using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto.Model.ViewModel;
using Proyecto.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;
using System.IO;
using Proyecto.Data;

namespace Proyecto.Pages
{
    [Authorize(Roles = "Enfermero")]
    public class EnfermeroModel : PageModel
    {
        private readonly ProyectoDbContext _context;

        public EnfermeroModel(ProyectoDbContext context)
        {
            _context = context;
        }

        public EnfermeroViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userDoc = User.Claims.FirstOrDefault(c => c.Type == "Documento")?.Value;
            var enfermero = await _context.Enfermeros
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Usuario.Documento == userDoc);

            if (enfermero == null)
            {
                return RedirectToPage("/Login");
            }

            var hoy = DateTime.Today;
            var primerDiaSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
            var diasSemana = Enumerable.Range(0, 7).Select(i => primerDiaSemana.AddDays(i)).ToList();

            // Solo citas del enfermero logueado
            var citasSemana = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Examen)
                .Include(c => c.Resultado)
                .Where(c => c.FechaHora.Date >= diasSemana.First()
                            && c.FechaHora.Date <= diasSemana.Last()
                            && c.Estado == EstadoGeneral.Activo
                            // && c.EnfermeroID == enfermero.EnfermeroID
                            )
                .OrderBy(c => c.FechaHora)
                .ToListAsync();

            var diasCalendario = diasSemana.Select(dia => new EnfermeroDiaCalendario
            {
                Fecha = dia,
                Citas = citasSemana
                    .Where(c => c.FechaHora.Date == dia.Date)
                    .Select(c => new EnfermeroCitaResumen
                    {
                        CitaID = c.CitaID,
                        Hora = c.FechaHora.TimeOfDay,
                        PacienteNombre = $"{c.Paciente.Nombres} {c.Paciente.Apellidos}",
                        ExamenNombre = c.Examen.Nombre
                    }).ToList()
            }).ToList();

            ViewModel = new EnfermeroViewModel
            {
                Anio = hoy.Year,
                Mes = hoy.Month,
                DiasSemana = diasCalendario
            };

            return Page();
        }

        // Handler para obtener detalle de cita en JSON
        public async Task<IActionResult> OnGetDetalleCitaAsync(int citaId)
        {
            var cita = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Examen)
                .Include(c => c.Resultado)
                .Include(c => c.EstadoCita)
                .FirstOrDefaultAsync(c => c.CitaID == citaId);

            if (cita == null) return NotFound();

            var detalle = new EnfermeroCitaDetalle
            {
                CitaID = cita.CitaID,
                FechaHora = cita.FechaHora,
                Estado = cita.EstadoCita?.Descripcion ?? cita.Estado.ToString(),
                PacienteID = cita.Paciente.PacienteID,
                PacienteNombre = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}",
                PacienteCorreo = cita.Paciente.Correo,
                PacienteTelefono = cita.Paciente.Telefono,
                PacienteDireccion = cita.Paciente.Direccion,
                PacienteNacimiento = cita.Paciente.Nacimiento,
                ExamenNombre = cita.Examen.Nombre,
                ExamenDescripcion = cita.Examen.Descripcion,
                ExamenIndicaciones = "", // No existe en modelo
                ResultadoRutaArchivo = cita.Resultado?.ArchivoPDF
            };

            return new JsonResult(detalle);
        }

        // Handler para subir archivo de resultado para una cita
        public async Task<IActionResult> OnPostSubirResultadoAsync(int citaId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["MensajeError"] = "Debes seleccionar un archivo PDF.";
                return RedirectToPage();
            }

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (extension != ".pdf")
            {
                TempData["MensajeError"] = "Solo se permiten archivos PDF.";
                return RedirectToPage();
            }

            var cita = await _context.Citas.Include(c => c.Resultado).FirstOrDefaultAsync(c => c.CitaID == citaId);
            if (cita == null)
            {
                TempData["MensajeError"] = "No se encontró la cita.";
                return RedirectToPage();
            }

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resultados");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);
            var rutaFisica = Path.Combine(rutaCarpeta, nombreArchivo);
            var rutaWeb = "/Resultados/" + nombreArchivo;

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            if (cita.Resultado == null)
            {
                cita.Resultado = new Resultado
                {
                    CitaID = cita.CitaID,
                    ArchivoPDF = rutaWeb,
                    FechaSubida = DateTime.Now,
                    Estado = EstadoGeneral.Activo
                };
                _context.Resultados.Add(cita.Resultado);
            }
            else
            {
                cita.Resultado.ArchivoPDF = rutaWeb;
                cita.Resultado.FechaSubida = DateTime.Now;
                cita.Resultado.Estado = EstadoGeneral.Activo;
            }

            await _context.SaveChangesAsync();
            TempData["MensajeExito"] = "El archivo se subió correctamente.";
            return RedirectToPage();
        }
    }
}