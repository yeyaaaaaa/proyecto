using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
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