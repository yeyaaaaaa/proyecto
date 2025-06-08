using Microsoft.AspNetCore.Mvc;
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
    public class RegistroModel : PageModel
    {
        private readonly ProyectoDbContext _context;
        public RegistroModel(ProyectoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegistroViewModel Registro { get; set; }

        public string Mensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Validar que el documento esté en Afiliaciones y que el estado sea Activo
            var afiliacion = _context.Afiliaciones.FirstOrDefault(a =>
                a.TipoDocumento == Registro.TipoDocumento
                && a.Documento == Registro.Documento
                && a.Estado == "Activo"
            );
            if (afiliacion == null)
            {
                Mensaje = "El documento ingresado no está afiliado a ninguna EPS aliada o la afiliación no está activa.";
                return Page();
            }

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

            // Hash de la contraseña
            var (hash, salt) = HashPassword(Registro.Contraseña);

            // Crear el Usuario (RolID paciente y Estado Activo)
            // Asume que el RolID '2' es Paciente
            var usuario = new Usuario
            {
                TipoDocumento = Registro.TipoDocumento,
                Documento = Registro.Documento,
                ContraseñaHash = hash,
                Salt = salt,
                RolID = 2,
                Estado = "Activo"
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear el Paciente y enlazar
            var paciente = new Paciente
            {
                Nombres = Registro.Nombres,
                Apellidos = Registro.Apellidos,
                Correo = Registro.Correo,
                Sexo = Registro.Sexo,
                Telefono = Registro.Telefono,
                Direccion = Registro.Direccion,
                Nacimiento = Registro.Nacimiento,
                UsuarioFK = usuario.UsuarioID,
                Estado = "Activo"
            };
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            // Redirigir a login o página de éxito
            return RedirectToPage("/Login");
        }

        // Hash + salt seguro para contraseñas
        private (string Hash, string Salt) HashPassword(string password)
        {
            // Genera un salt aleatorio
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Hash con PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 256 bits
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }
    }
}