using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Cita
    {
        [Key]
        public int CitaID { get; set; }

        [Required]
        [ForeignKey("Paciente")]
        public int PacienteID { get; set; }

        [Required]
        [ForeignKey("Examen")]
        public int ExamenID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [Required]
        [ForeignKey("EstadoCita")]
        public int EstadoID { get; set; }

        [Required]
        [MaxLength(1)]
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