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

        public IActionResult OnPostRegistrarPaciente()
        {
            if (string.IsNullOrWhiteSpace(AdminVM.PacienteTipoDocumento) || string.IsNullOrWhiteSpace(AdminVM.PacienteDocumento) || AdminVM.PacienteEPSID == null)
            {
                ModelState.AddModelError(string.Empty, "Todos los campos de paciente son obligatorios");
                CargarListados();
                AdminVM.EPSs = _context.EPSs.Where(e => e.Estado == EstadoGeneral.Activo).ToList();
                return Page();
            }

            // Verifica si ya existe un paciente con ese documento
            bool existe = _context.Pacientes.Any(p =>
                p.Usuario.TipoDocumento == AdminVM.PacienteTipoDocumento &&
                p.Usuario.Documento == AdminVM.PacienteDocumento);

            if (existe)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un paciente con ese documento.");
                CargarListados();
                AdminVM.EPSs = _context.EPSs.Where(e => e.Estado == EstadoGeneral.Activo).ToList();
                return Page();
            }

            // Crea el usuario mínimo para el paciente (sin contraseña aún)
            var usuario = new Usuario
            {
                TipoDocumento = AdminVM.PacienteTipoDocumento,
                Documento = AdminVM.PacienteDocumento,
                ContraseñaHash = "", // El usuario la creará luego
                Salt = "",
                RolID = _context.Roles.FirstOrDefault(r => r.Nombre == "Paciente")?.RolID ?? 3,
                Estado = EstadoGeneral.Inactivo // Inactivo hasta que el usuario se registre
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Crea el paciente
            var paciente = new Paciente
            {
                Nombres = "",
                Apellidos = "",
                Correo = "",
                Sexo = "",
                Telefono = "",
                Direccion = "",
                Nacimiento = new System.DateTime(1900, 1, 1),
                UsuarioFK = usuario.UsuarioID,
                Estado = EstadoGeneral.Inactivo
            };
            _context.Pacientes.Add(paciente);
            _context.SaveChanges();

            // Crea la afiliación a la EPS
            var afiliacion = new Afiliacion
            {
                TipoDocumento = AdminVM.PacienteTipoDocumento,
                Documento = AdminVM.PacienteDocumento,
                EPSID = AdminVM.PacienteEPSID.Value,
                Estado = EstadoGeneral.Activo
            };
            _context.Afiliaciones.Add(afiliacion);
            _context.SaveChanges();

            TempData["Mensaje"] = "Paciente añadido correctamente. Ahora puede registrarse como usuario.";
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