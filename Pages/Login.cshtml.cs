using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Proyecto.Model;
using Proyecto.Model.ViewModel;
using Proyecto.Data;

namespace Proyecto.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ProyectoDbContext _context;

        public LoginModel(ProyectoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Busca usuario por documento exacto
            var user = _context.Usuarios
                        .Include(u => u.Rol)
                        .FirstOrDefault(u => u.Documento == Login.Documento);

            if (user == null || !VerificarContraseña(Login.Password, user.ContraseñaHash, user.Salt))
            {
                ErrorMessage = "Documento o contraseña incorrectos";
                return Page();
            }

            // Verifica si el usuario está activo
            if (user.Estado != EstadoGeneral.Activo)
            {
                ErrorMessage = "El usuario no está activo.";
                return Page();
            }

            // Historial de login
            bool loginExitoso = user != null && VerificarContraseña(Login.Password, user.ContraseñaHash, user.Salt) && user.Estado == EstadoGeneral.Activo;

            var log = new LogSistema
            {
                UsuarioID = user?.UsuarioID ?? 0, 
                Accion = loginExitoso ? "Login exitoso" : "Login fallido",
                FechaHora = DateTime.Now,
                Estado = loginExitoso ? EstadoGeneral.Activo : EstadoGeneral.Inactivo
            };
            _context.LogsSistema.Add(log);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Documento),
                new Claim("Documento", user.Documento),
                new Claim(ClaimTypes.Role, user.Rol.Nombre)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redirección según rol
            switch (user.Rol.Nombre.ToLower())
            {
                case "paciente":
                    return RedirectToPage("/IndexPaciente");
                case "enfermero":
                    return RedirectToPage("/Enfermero/Index");
                case "administrador":
                    return RedirectToPage("/Admin/Index");
                default:
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    ErrorMessage = "Rol no reconocido";
                    return Page();
            }
        }

        // PBKDF2: Verifica el password usando el mismo algoritmo que en el registro
        private bool VerificarContraseña(string password, string hashAlmacenado, string saltBase64)
        {
            // Decodifica el salt de base64
            byte[] saltBytes = Convert.FromBase64String(saltBase64);

            // Aplica PBKDF2 con SHA256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 256 bits
                string hashIngresado = Convert.ToBase64String(hashBytes);
                return hashIngresado == hashAlmacenado;
            }
        }
    }
}