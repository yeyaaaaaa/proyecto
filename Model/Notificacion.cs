using System;
using System.Collections.Generic;

namespace Proyecto.Model
{
    public class Notificacion
    {
        public int NotificacionID { get; set; }
        public int UsuarioID { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}