using System;
using System.Collections.Generic;

namespace Proyecto.Model.ViewModel
{
    // Citas resumidas para mostrar en el calendario (por día)
    public class EnfermeroCitaResumen
    {
        public int CitaID { get; set; }
        public TimeSpan Hora { get; set; }
        public string PacienteNombre { get; set; }
        public string ExamenNombre { get; set; }
    }

    // Detalle completo de la cita seleccionada (todo sale de las tablas ya existentes)
    public class EnfermeroCitaDetalle
    {
        public int CitaID { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }

        // Datos del paciente (tabla Paciente)
        public int PacienteID { get; set; }
        public string PacienteNombre { get; set; }
        public string PacienteCorreo { get; set; }
        public string PacienteTelefono { get; set; }
        public string PacienteDireccion { get; set; }
        public DateTime PacienteNacimiento { get; set; }

        // Datos del examen (tabla Examen)
        public string ExamenNombre { get; set; }
        public string ExamenDescripcion { get; set; }
        public string ExamenIndicaciones { get; set; }

        // Resultado (tabla Resultado, si ya se subió)
        public string ResultadoRutaArchivo { get; set; }
    }

    // Día del calendario con sus citas
    public class EnfermeroDiaCalendario
    {
        public DateTime Fecha { get; set; }
        public List<EnfermeroCitaResumen> Citas { get; set; } = new();
    }

    // ViewModel principal del calendario semanal del enfermero
    public class EnfermeroViewModel
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public List<EnfermeroDiaCalendario> DiasSemana { get; set; } = new();

        // Detalle de la cita seleccionada (para el modal o panel que se despliega)
        public EnfermeroCitaDetalle? CitaSeleccionada { get; set; }
    }
}