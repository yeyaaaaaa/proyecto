using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Resultado
    {
        [Key]
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
        [MaxLength(1)]
        public string Estado { get; set; }

        public Cita Cita { get; set; }
    }
}