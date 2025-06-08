using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class EstadoCita
    {
        public int EstadoID { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Cita> Citas { get; set; }
        public ICollection<HistorialCita> HistorialCitasAnterior { get; set; }
        public ICollection<HistorialCita> HistorialCitasNuevo { get; set; }
    
        public EstadoCita()
        {
            Citas = new HashSet<Cita>();
            HistorialCitasAnterior = new HashSet<HistorialCita>();
            HistorialCitasNuevo = new HashSet<HistorialCita>();
        }
    }
}