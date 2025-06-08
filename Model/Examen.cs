using System;
using System.Collections.Generic;

namespace Proyecto.Model
{
    public class Examen
    {
        public int ExamenID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string Estado { get; set; }

        public ICollection<Cita> Citas { get; set; }
    
        public Examen()
        {
            Citas = new HashSet<Cita>();
        }
    }
}