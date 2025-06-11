using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Examen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamenID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public ICollection<Cita> Citas { get; set; }
    
        public Examen()
        {
            Citas = new HashSet<Cita>();
        }
    }
}