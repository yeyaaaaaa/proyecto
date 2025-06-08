using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class Reprogramacion
    {
        public int ReprogramacionID { get; set; }
        public int CitaID { get; set; }
        public DateTime NuevaFecha { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }

        public Cita Cita { get; set; }
    }
}