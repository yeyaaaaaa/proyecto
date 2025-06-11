using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Afiliacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AfiliacionID { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public int EPSID { get; set; }

        [Required]
        public EstadoGeneral Estado { get; set; }

        public EPS EPS { get; set; }
    }
}