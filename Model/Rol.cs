using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
        
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }
    }
}