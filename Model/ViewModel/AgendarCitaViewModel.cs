using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Model.ViewModel
{
    public class AgendarCitaViewModel
    {
        [Required]
        public int ExamenID { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        public List<Examen> ExamenesDisponibles { get; set; } = new();
        public List<DateTime> DiasLlenos { get; set; } = new();
        public List<DateTime> DiasDisponibles { get; set; } = new();
        public List<TimeSpan> HorasDisponibles { get; set; } = new();
    }
}