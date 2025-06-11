using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Notificacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificacionID { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [Required]
        public string Mensaje { get; set; }

        [Required]
        [MaxLength(1)]
        public bool Leida { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public Usuario Usuario { get; set; }
    }
}