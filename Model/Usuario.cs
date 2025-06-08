using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El documento debe tener entre 6 y 20 caracteres")]
        public string Documento { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", 
        ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, un número y un símbolo")]
        public string ContraseñaHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public int RolID { get; set; }

        [Required]
        public string Estado { get; set; }

        public Rol Rol { get; set; }
        public ICollection<Notificacion> Notificaciones { get; set; }
        public ICollection<LogSistema> Logs { get; set; }

        public Usuario()
        {
            Notificaciones = new HashSet<Notificacion>();
            Logs = new HashSet<LogSistema>();
        }
    }
}