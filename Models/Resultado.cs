using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class Resultado
    {
        public int ResultadoID { get; set; }
        public int CitaID { get; set; }
        public string ArchivoPDF { get; set; }
        public DateTime FechaSubida { get; set; }
        public string Estado { get; set; }

        public Cita Cita { get; set; }
    }
}