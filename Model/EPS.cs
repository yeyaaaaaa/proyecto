using System;
using System.Collections.Generic;

namespace Proyecto.Model
{
    public class EPS
    {
        public int EPSID { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }

        public ICollection<Afiliacion> Afiliaciones { get; set; }
    
        public EPS()
        {
            Afiliaciones = new HashSet<Afiliacion>();
        }
    }
}