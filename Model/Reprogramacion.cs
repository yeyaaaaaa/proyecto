using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Reprogramacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public EstadoGeneral Estado { get; set; }

        public Cita Cita { get; set; }
    }
}