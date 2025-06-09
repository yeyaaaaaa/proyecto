using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proyecto.Model.ViewModel;
using Proyecto.Data;
using System.Threading.Tasks;
using Proyecto.Model;
using System.Linq;
using System;

namespace Proyecto.Pages
{
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
            // Obtén el usuario logueado
            var userDoc = User.Claims.FirstOrDefault(c => c.Type == "Documento")?.Value;
            if (string.IsNullOrEmpty(userDoc))
                return RedirectToPage("/Login");

            // Busca el paciente con su usuario
            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Usuario.Documento == userDoc);

            if (paciente == null)
                return NotFound();

            // Lógica para actualizar TI a CC si corresponde
            var today = DateTime.Today;
            var edad = today.Year - paciente.Nacimiento.Year;
            if (paciente.Nacimiento.Date > today.AddYears(-edad)) edad--;

            if (paciente.Usuario.TipoDocumento == "TI" && edad >= 18)
            {
                paciente.Usuario.TipoDocumento = "CC";
                await _context.SaveChangesAsync();
                Mensaje = "Su tipo de documento ha sido actualizado automáticamente a CC por mayoría de edad.";
            }

            // Busca la afiliación activa y la EPS
            var afiliacion = await _context.Afiliaciones
                .Include(a => a.EPS)
                .FirstOrDefaultAsync(a => a.Documento == paciente.Usuario.Documento && a.Estado == EstadoGeneral.Activo);

            Perfil = new PacientePerfilViewModel
            {
                PacienteID = paciente.PacienteID,
                Nombres = paciente.Nombres,
                Apellidos = paciente.Apellidos,
                Correo = paciente.Correo,
                Sexo = paciente.Sexo,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                Nacimiento = paciente.Nacimiento,
                TipoDocumento = paciente.Usuario.TipoDocumento,
                Documento = paciente.Usuario.Documento,
                NombreEPS = afiliacion?.EPS?.Nombre ?? "No afiliado"
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.PacienteID == Perfil.PacienteID);

            if (paciente == null)
                return NotFound();

            // Solo actualiza los campos permitidos
            paciente.Nombres = Perfil.Nombres;
            paciente.Apellidos = Perfil.Apellidos;
            paciente.Correo = Perfil.Correo;
            paciente.Sexo = Perfil.Sexo;
            paciente.Telefono = Perfil.Telefono;
            paciente.Direccion = Perfil.Direccion;
            // paciente.Nacimiento NO se actualiza aquí

            await _context.SaveChangesAsync();
            Mensaje = "Información actualizada correctamente.";
            return RedirectToPage();
        }
    }
}