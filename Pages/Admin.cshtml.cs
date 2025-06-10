using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto.Data;
using Proyecto.Model.ViewModel;
using Proyecto.Model;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class AdminModel : PageModel
    {
        private readonly ProyectoDbContext _context;

        public AdminModel(ProyectoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdminViewModel AdminVM { get; set; } = new();

        public void OnGet()
        {
            CargarListados();
            AdminVM.EPSs = _context.EPSs.Where(e => e.Estado == EstadoGeneral.Activo).ToList();
        }

        // Corregido: Solo crea la afiliación, no usuario ni paciente
        public IActionResult OnPostRegistrarPaciente()
        {
            if (string.IsNullOrWhiteSpace(AdminVM.PacienteTipoDocumento) || string.IsNullOrWhiteSpace(AdminVM.PacienteDocumento) || AdminVM.PacienteEPSID == null)
            {
                ModelState.AddModelError(string.Empty, "Todos los campos de paciente son obligatorios");
                CargarListados();
                AdminVM.EPSs = _context.EPSs.Where(e => e.Estado == EstadoGeneral.Activo).ToList();
                return Page();
            }

            // Verifica si ya existe una afiliación con ese documento y EPS activa
            bool existeAfiliacion = _context.Afiliaciones.Any(a =>
                a.TipoDocumento == AdminVM.PacienteTipoDocumento &&
                a.Documento == AdminVM.PacienteDocumento &&
                a.EPSID == AdminVM.PacienteEPSID.Value &&
                a.Estado == EstadoGeneral.Activo
            );

            if (existeAfiliacion)
            {
                ModelState.AddModelError(string.Empty, "Ya existe una afiliación activa para ese documento y EPS.");
                CargarListados();
                AdminVM.EPSs = _context.EPSs.Where(e => e.Estado == EstadoGeneral.Activo).ToList();
                return Page();
            }

            // Solo crea la afiliación
            var afiliacion = new Afiliacion
            {
                TipoDocumento = AdminVM.PacienteTipoDocumento,
                Documento = AdminVM.PacienteDocumento,
                EPSID = AdminVM.PacienteEPSID.Value,
                Estado = EstadoGeneral.Activo
            };
            _context.Afiliaciones.Add(afiliacion);
            _context.SaveChanges();

            TempData["Mensaje"] = "Afiliación creada correctamente. El usuario puede ahora registrarse.";
            return RedirectToPage();
        }

        public IActionResult OnPostCambiarEstadoEnfermero(int EnfermeroID, EstadoGeneral NuevoEstado)
        {
            var enfermero = _context.Enfermeros.FirstOrDefault(e => e.EnfermeroID == EnfermeroID);
            if (enfermero != null)
            {
                enfermero.Estado = NuevoEstado;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostCambiarEstadoPaciente(int PacienteID, EstadoGeneral NuevoEstado)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.PacienteID == PacienteID);
            if (paciente != null)
            {
                paciente.Estado = NuevoEstado;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private void CargarListados()
        {
            AdminVM.Enfermeros = _context.Enfermeros
                .Select(e => new EnfermeroEstadoViewModel
                {
                    EnfermeroID = e.EnfermeroID,
                    Nombres = e.Nombres,
                    Apellidos = e.Apellidos,
                    Documento = e.Usuario.Documento,
                    Correo = e.Correo,
                    Telefono = e.Telefono,
                    Estado = e.Estado
                })
                .ToList();

            AdminVM.Pacientes = _context.Pacientes
                .Select(p => new PacienteEstadoViewModel
                {
                    PacienteID = p.PacienteID,
                    Nombres = p.Nombres,
                    Apellidos = p.Apellidos,
                    Documento = p.Usuario.Documento,
                    Correo = p.Correo,
                    Estado = p.Estado
                })
                .ToList();
        }
    }
}