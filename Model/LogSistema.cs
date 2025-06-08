using System;
using System.Collections.Generic;

namespace Proyecto.Model
{
    public class LogSistema
    {
        public int LogID { get; set; }
        public int UsuarioID { get; set; }
        public string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}