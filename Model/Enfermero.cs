using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Enfermero
    {
        [Key]
        public int EnfermeroID { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioFK { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}