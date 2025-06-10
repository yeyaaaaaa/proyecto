using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Model;
using Proyecto.Model.ViewModel;
using Proyecto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Pages
{
    [Authorize(Roles = "Paciente")]
    public class AgendarCitaModel : PageModel
    {
        private readonly ProyectoDbContext _context;
        private readonly EmailService _emailService;
        private const int CitasPorEnfermeroPorDia = 12;
        private static readonly TimeSpan HoraInicio = new(6, 0, 0);
        private static readonly TimeSpan HoraFin = new(12, 0, 0);
        private static readonly TimeSpan DuracionCita = new(0, 30, 0);
        [TempData]
        public string MensajeTemp { get; set; }

        public AgendarCitaModel(ProyectoDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public AgendarCitaViewModel DatosCita { get; set; } = new AgendarCitaViewModel();

        public string Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarDatosCalendario();
            return Page();
        }

        public async Task<JsonResult> OnGetDiasDisponiblesAsync(int examenId)
        {
            await CargarDatosCalendario(examenId);
            return new JsonResult(DatosCita);
        }

        public async Task<JsonResult> OnGetHorasDisponiblesAsync(string fecha, int examenId)
        {
            if (!DateTime.TryParse(fecha, out var fechaDT))
                return new JsonResult(new List<TimeSpan>());
            var horasDisponibles = await CalcularHorasDisponibles(fechaDT, examenId);
            return new JsonResult(horasDisponibles);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarDatosCalendario(DatosCita.ExamenID);

            // Validación de fecha y hora válidas
            if (DatosCita.Fecha == DateTime.MinValue || DatosCita.Hora == TimeSpan.Zero || DatosCita.ExamenID == 0)
            {
                Mensaje = "Debe seleccionar una fecha y una hora válidas.";
                return Page();
            }

            var userDoc = User.Claims.FirstOrDefault(c => c.Type == "Documento")?.Value;
            var paciente = await _context.Pacientes.Include(p => p.Usuario)
                                    .FirstOrDefaultAsync(p => p.Usuario.Documento == userDoc);
            if (paciente == null)
                return RedirectToPage("/Login");

            var fechaHoraCita = DatosCita.Fecha.Date + DatosCita.Hora;
            var limiteSemana = fechaHoraCita.AddDays(-7);

            var yaTieneCita = await _context.Citas
                .AnyAsync(c => c.PacienteID == paciente.PacienteID &&
                            c.ExamenID == DatosCita.ExamenID &&
                            c.FechaHora >= limiteSemana &&
                            c.FechaHora <= fechaHoraCita &&
                            c.Estado == EstadoGeneral.Activo);

            if (yaTieneCita)
            {
                Mensaje = "No puede agendar el mismo examen más de una vez en una semana.";
                return Page();
            }

            var horasDisponibles = await CalcularHorasDisponibles(DatosCita.Fecha, DatosCita.ExamenID);
            if (!horasDisponibles.Contains(DatosCita.Hora))
            {
                Mensaje = "El horario seleccionado ya no está disponible. Por favor, elija otro.";
                return Page();
            }

            // --- ASIGNACIÓN AUTOMÁTICA Y EQUITATIVA DE ENFERMERO ---
            var enfermerosActivos = await _context.Enfermeros
                .Include(e => e.Usuario)
                .Where(e => e.Usuario.Estado == EstadoGeneral.Activo)
                .ToListAsync();

            if (!enfermerosActivos.Any())
            {
                Mensaje = "No hay enfermeros disponibles para asignar la cita.";
                return Page();
            }

            // Busca el enfermero con menos citas asignadas en esa fecha/hora
            var enfermeroConMenosCitas = enfermerosActivos
                .Select(e => new
                {
                    Enfermero = e,
                    CitasCount = _context.Citas.Count(c =>
                        c.EnfermeroID == e.EnfermeroID &&
                        c.FechaHora.Date == DatosCita.Fecha.Date &&
                        c.FechaHora.TimeOfDay == DatosCita.Hora &&
                        c.Estado == EstadoGeneral.Activo)
                })
                .OrderBy(x => x.CitasCount)
                .ThenBy(x => x.Enfermero.EnfermeroID)
                .First()
                .Enfermero;

            // --- CREACIÓN DE LA CITA ---
            var cita = new Cita
            {
                PacienteID = paciente.PacienteID,
                ExamenID = DatosCita.ExamenID,
                FechaHora = fechaHoraCita,
                EstadoID = await _context.Estados
                                    .Where(e => e.Descripcion == "Reservada")
                                    .Select(e => e.EstadoID)
                                    .FirstOrDefaultAsync(),
                Estado = EstadoGeneral.Activo,
                EnfermeroID = enfermeroConMenosCitas.EnfermeroID // Asignación automática aquí
            };
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            await EnviarCorreoConfirmacion(paciente, cita);

            MensajeTemp = "¡Cita agendada exitosamente! Revise su correo para los detalles y recomendaciones.";
            return RedirectToPage();
        }

        private async Task CargarDatosCalendario(int? examenId = null)
        {
            // Solo inicializa si está en null
            if (DatosCita == null)
                DatosCita = new AgendarCitaViewModel();

            DatosCita.ExamenesDisponibles = await _context.Examenes
                .Where(e => e.Estado == EstadoGeneral.Activo)
                .ToListAsync();

            var hoy = DateTime.Today;
            var desde = hoy.AddDays(1);
            var hasta = hoy.AddMonths(1);

            var enfermerosActivos = await _context.Enfermeros
                .Include(e => e.Usuario)
                .CountAsync(e => e.Usuario.Estado == EstadoGeneral.Activo);

            int cupoPorDia = enfermerosActivos * CitasPorEnfermeroPorDia;
            var diasLlenos = new List<DateTime>();
            var diasDisponibles = new List<DateTime>();

            for (var dia = desde; dia <= hasta; dia = dia.AddDays(1))
            {
                var totalCitasDia = await _context.Citas
                    .CountAsync(c => c.FechaHora.Date == dia && c.Estado == EstadoGeneral.Activo);

                if (totalCitasDia >= cupoPorDia)
                    diasLlenos.Add(dia);
                else
                    diasDisponibles.Add(dia);
            }

            DatosCita.DiasLlenos = diasLlenos;
            DatosCita.DiasDisponibles = diasDisponibles;
        }

        private async Task<List<TimeSpan>> CalcularHorasDisponibles(DateTime fecha, int examenId)
        {
            var horas = new List<TimeSpan>();
            var enfermerosActivos = await _context.Enfermeros
                .Include(e => e.Usuario)
                .CountAsync(e => e.Usuario.Estado == EstadoGeneral.Activo);

            int cupoPorHora = enfermerosActivos;

            for (var hora = HoraInicio; hora < HoraFin; hora += DuracionCita)
            {
                var citasEnHora = await _context.Citas
                    .CountAsync(c => c.FechaHora.Date == fecha.Date
                                     && c.FechaHora.TimeOfDay == hora
                                     && c.Estado == EstadoGeneral.Activo);
                if (citasEnHora < cupoPorHora)
                    horas.Add(hora);
            }
            return horas;
        }

        private async Task EnviarCorreoConfirmacion(Paciente paciente, Cita cita)
        {
            var examen = await _context.Examenes.FindAsync(cita.ExamenID);
            var correo = paciente.Correo;
            var asunto = "Confirmación de cita reservada";
            var cuerpo = $@"
                <h3>Su cita ha sido reservada exitosamente</h3>
                <p><b>Fecha:</b> {cita.FechaHora:yyyy-MM-dd}</p>
                <p><b>Hora:</b> {cita.FechaHora:HH:mm}</p>
                <p><b>Examen:</b> {examen?.Nombre}</p>
                <p><b>Paciente:</b> {paciente.Nombres} {paciente.Apellidos}</p>
                <p>Por favor, presentar su orden médica el día de la cita y asistir con ayuno si es requerido por el examen.</p>
                <p>¡Gracias por confiar en nuestro laboratorio!</p>";

            await _emailService.SendEmailAsync(correo, asunto, cuerpo);
        }
    }
}