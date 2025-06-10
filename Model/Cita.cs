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
        public EstadoGeneral Estado { get; set; }

        // NUEVAS LINEAS: Relación con Enfermero
        [ForeignKey("Enfermero")]
        public int? EnfermeroID { get; set; } // Puede ser null si la cita aún no está asignada

        public Paciente Paciente { get; set; }
        public Examen Examen { get; set; }
        public EstadoCita EstadoCita { get; set; }
        public Enfermero Enfermero { get; set; } // Navegación a enfermero

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