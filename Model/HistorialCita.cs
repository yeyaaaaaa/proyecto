using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class HistorialCita
    {
        [Key]
        public int HistorialID { get; set; }

        [Required]
        [ForeignKey("Cita")]
        public int CitaID { get; set; }

        [Required]
        [ForeignKey("EstadoAnteriorNavigation")]
        public int EstadoAnterior { get; set; }

        [Required]
        public int EstadoNuevo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaCambio { get; set; }

        public Cita Cita { get; set; }
        public EstadoCita EstadoAnteriorNavigation { get; set; }
        public EstadoCita EstadoNuevoNavigation { get; set; }
    }
}