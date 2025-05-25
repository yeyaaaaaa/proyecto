using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using proyecto.Data;
using proyecto.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MyApp.Namespace
{
    public class RegistroModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegistroModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario Usuario { get; set; }
        [BindProperty]
        public Paciente Paciente { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool estaAfiliado = await _context.Afiliaciones
                .AnyAsync(a => a.Usuario != null && a.Usuario.Documento == Usuario.Documento);

            if (!estaAfiliado)
            {
                ModelState.AddModelError(string.Empty, "Usted no está afiliado a una EPS registrada.");
                return Page();
            }

            // Contar cuántos usuarios ya tienen el mismo Documento
            int usuariosConDocumento = await _context.Usuarios
                .CountAsync(u => u.Documento == Usuario.Documento);

            if (usuariosConDocumento >= 2)
            {
                // Agregar error al modelo para mostrarlo en la vista
                ModelState.AddModelError("Usuario.Documento", "No se pueden crear más de dos usuarios con el mismo documento.");
                return Page();
            }

            // Asignar el rol de paciente
            Usuario.RolFK = 2;

            Usuario.Estado = true;
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();

            Paciente.UsuarioFK = Usuario.UsuarioID;
            _context.Pacientes.Add(Paciente);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }

    }
}
