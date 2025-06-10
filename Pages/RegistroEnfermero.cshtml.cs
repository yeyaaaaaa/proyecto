using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto.Model.ViewModel;
using Proyecto.Data;
using Proyecto.Model;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto.Pages
{
    [Authorize(Roles = "Administrador")]
    public class RegistroEnfermeroModel : PageModel
    {
        private readonly ProyectoDbContext _context;
        public RegistroEnfermeroModel(ProyectoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegistroEnfermeroViewModel Registro { get; set; }

        public string Mensaje { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Validar que el documento NO exista ya en Usuario
            var existeUsuario = _context.Usuarios.Any(u =>
                u.TipoDocumento == Registro.TipoDocumento &&
                u.Documento == Registro.Documento
            );
            if (existeUsuario)
            {
                Mensaje = "Ya existe un usuario registrado con ese número de documento.";
                return Page();
            }

            // Validar correo único
            var existeCorreo = _context.Enfermeros.Any(e => e.Correo == Registro.Correo);
            if (existeCorreo)
            {
                Mensaje = "Ya existe un enfermero registrado con ese correo.";
                return Page();
            }

            // Hash de la contraseña
            var (hash, salt) = HashPassword(Registro.Contraseña);

            // Crear el Usuario (RolID 3 = Enfermero, Estado Activo)
            var usuario = new Usuario
            {
                TipoDocumento = Registro.TipoDocumento,
                Documento = Registro.Documento,
                ContraseñaHash = hash,
                Salt = salt,
                RolID = 3,
                Estado = EstadoGeneral.Activo
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear el Enfermero y enlazar
            var enfermero = new Enfermero
            {
                Nombres = Registro.Nombres,
                Apellidos = Registro.Apellidos,
                Correo = Registro.Correo,
                Telefono = Registro.Telefono,
                UsuarioFK = usuario.UsuarioID,
                Estado = EstadoGeneral.Activo
            };
            _context.Enfermeros.Add(enfermero);
            await _context.SaveChangesAsync();

            // Redirigir a login o página de éxito
            return RedirectToPage("/Login");
        }

        // Hash + salt seguro para contraseñas
        private (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }
    }
}