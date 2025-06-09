using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Paciente
    {
        [Key]
        public int PacienteID { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Sexo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public DateTime Nacimiento { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioFK { get; set; }

        [Required]
        public string Estado { get; set; }

        public virtual Usuario Usuario { get; set; }
        public ICollection<Cita> Citas { get; set; }
        
        public Paciente()
        {
            Citas = new HashSet<Cita>();
        }
    }
}