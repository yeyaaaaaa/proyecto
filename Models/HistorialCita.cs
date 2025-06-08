using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class HistorialCita
    {
        public int HistorialID { get; set; }
        public int CitaID { get; set; }
        public int EstadoAnterior { get; set; }
        public int EstadoNuevo { get; set; }
        public DateTime FechaCambio { get; set; }

        public Cita Cita { get; set; }
        public EstadoCita EstadoAnteriorNavigation { get; set; }
        public EstadoCita EstadoNuevoNavigation { get; set; }
    }
}