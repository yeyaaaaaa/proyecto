using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class LogSistema
    {
        [Key]
        public int LogID { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [Required]
        public string Accion { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [Required]
        [MaxLength(1)]
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}