using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto.Models
{
    public class Afiliacion
    {
        [Key]
        public int AfiliacionID { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public int EPSID { get; set; }

        [Required]
        public string Estado { get; set; }

        public EPS EPS { get; set; }
    }
}