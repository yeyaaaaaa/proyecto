using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Proyecto.Model;

namespace Proyecto.Model.ViewModel
{
    public class AdminViewModel
    {
        // Registro paciente mínimo
        [Display(Name = "Tipo de documento")]
        public string PacienteTipoDocumento { get; set; }
        [Display(Name = "Número de documento")]
        public string PacienteDocumento { get; set; }
        [Display(Name = "EPS afiliada")]
        public int? PacienteEPSID { get; set; }
        // Para el select de EPS
        public List<EPS> EPSs { get; set; } = new();

        public List<EnfermeroEstadoViewModel> Enfermeros { get; set; } = new();
        public List<PacienteEstadoViewModel> Pacientes { get; set; } = new();
    }

    public class EnfermeroEstadoViewModel
    {
        public int EnfermeroID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public EstadoGeneral Estado { get; set; }
    }

    public class PacienteEstadoViewModel
    {
        public int PacienteID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Correo { get; set; }
        public EstadoGeneral Estado { get; set; }
    }
}