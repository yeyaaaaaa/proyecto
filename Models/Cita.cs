using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class Cita
    {
        public int CitaID { get; set; }
        public int PacienteID { get; set; }
        public int ExamenID { get; set; }
        public DateTime FechaHora { get; set; }
        public int EstadoID { get; set; }
        public string Estado { get; set; }

        public Paciente Paciente { get; set; }
        public Examen Examen { get; set; }
        public EstadoCita EstadoCita { get; set; }
        public ICollection<Reprogramacion> Reprogramaciones { get; set; }
        public Resultado Resultado { get; set; }
        public ICollection<HistorialCita> HistorialCitas { get; set; }
   
        public Cita()
        {
            Reprogramaciones = new HashSet<Reprogramacion>();
            HistorialCitas = new HashSet<HistorialCita>();
        }
    }
}