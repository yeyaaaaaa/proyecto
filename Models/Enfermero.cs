using System;
using System.Collections.Generic;

namespace proyecto.Models
{
    public class Enfermero
    {
        public int EnfermeroID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int UsuarioFK { get; set; }
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}