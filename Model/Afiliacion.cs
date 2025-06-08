using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Model
{
    public class Afiliacion
    {
        [Key]
        public int AfiliacionID { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El documento debe tener entre 6 y 20 caracteres")]
        public string Documento { get; set; }

        [Required]
        public int EPSID { get; set; }

        [Required]
        public string Estado { get; set; }

        public EPS EPS { get; set; }
    }
}