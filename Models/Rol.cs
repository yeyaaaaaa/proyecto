using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class Rol
    {
        public int RolID { get; set; }
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
        
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }
    }
}