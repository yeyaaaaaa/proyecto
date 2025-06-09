using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class EPS
    {
        [Key]
        public int EPSID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public ICollection<Afiliacion> Afiliaciones { get; set; }
    
        public EPS()
        {
            Afiliaciones = new HashSet<Afiliacion>();
        }
    }
}