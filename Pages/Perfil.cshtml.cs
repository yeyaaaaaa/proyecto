using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto.Model.ViewModel;
using Proyecto.Data;
using System.Threading.Tasks;
using Proyecto.Model;
using System.Linq;
using System;

namespace Proyecto.Pages
{
    [Authorize]
    public class PerfilModel : PageModel
    {
        private readonly ProyectoDbContext _context;

        public PerfilModel(ProyectoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PacientePerfilViewModel Perfil { get; set; }

        [TempData]
        public string Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarPerfil();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Mensaje = "OnPostAsync ejecutado";  // <- Debug

            if (!ModelState.IsValid)
            {
                Mensaje = "Error de validación";
                await CargarPerfil();
                return Page();
            }

            var userDoc = User.Claims.FirstOrDefault(c => c.Type == "Documento")?.Value;
            if (string.IsNullOrEmpty(userDoc))
                return RedirectToPage("/Login");

            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Usuario.Documento == userDoc);

            if (paciente == null)
                return NotFound();

            // Actualiza campos permitidos
            paciente.Nombres = Perfil.Nombres;
            paciente.Apellidos = Perfil.Apellidos;
            paciente.Correo = Perfil.Correo;
            paciente.Sexo = Perfil.Sexo switch
            {
                "Masculino" => "M",
                "Femenino" => "F",
                "Otro" => "Otro",
                _ => ""
            };
            paciente.Telefono = Perfil.Telefono;
            paciente.Direccion = Perfil.Direccion;

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                Mensaje = "Información actualizada correctamente.";
            }
            catch (Exception ex)
            {
                Mensaje = "Error al guardar: " + ex.Message;
            }

            await CargarPerfil();
            return Page();
        }

        // Método auxiliar para recargar el perfil del usuario autenticado
        private async Task CargarPerfil()
        {
            var userDoc = User.Claims.FirstOrDefault(c => c.Type == "Documento")?.Value;
            if (string.IsNullOrEmpty(userDoc))
            {
                Perfil = new PacientePerfilViewModel();
                return;
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Usuario.Documento == userDoc);

            if (paciente == null)
            {
                Perfil = new PacientePerfilViewModel();
                return;
            }

            var afiliacion = await _context.Afiliaciones
                .Include(a => a.EPS)
                .FirstOrDefaultAsync(a => a.Documento == paciente.Usuario.Documento && a.Estado == EstadoGeneral.Activo);

            Perfil = new PacientePerfilViewModel
            {
                PacienteID = paciente.PacienteID,
                Nombres = paciente.Nombres,
                Apellidos = paciente.Apellidos,
                Correo = paciente.Correo,
                Sexo = paciente.Sexo switch
                {
                    "M" => "Masculino",
                    "F" => "Femenino",
                    "Otro" => "Otro",
                    _ => ""
                },
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                Nacimiento = paciente.Nacimiento,
                TipoDocumento = paciente.Usuario.TipoDocumento,
                Documento = paciente.Usuario.Documento,
                NombreEPS = afiliacion?.EPS?.Nombre ?? "No afiliado"
            };
        }
    }
}