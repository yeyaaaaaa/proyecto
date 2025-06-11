using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Resultado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultadoID { get; set; }

        [Required]
        [ForeignKey("Cita")]
        public int CitaID { get; set; }

        [Required]
        public string ArchivoPDF { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaSubida { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public Cita Cita { get; set; }
    }
}