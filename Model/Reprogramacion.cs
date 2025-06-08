using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Reprogramacion
    {
        [Key]
        public int ReprogramacionID { get; set; }

        [Required]
        [ForeignKey("Cita")]
        public int CitaID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime NuevaFecha { get; set; }

        [Required]
        public string Motivo { get; set; }

        [Required]
        [MaxLength(1)]
        public string Estado { get; set; }

        public Cita Cita { get; set; }
    }
}