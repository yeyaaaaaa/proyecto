using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class EstadoCita
    {
        [Key]
        public int EstadoID { get; set; }

        [Required]
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