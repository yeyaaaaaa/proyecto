using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Model.ViewModel
{
    public class PacientePerfilViewModel
    {
        public int PacienteID { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Sexo { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Ingerese un número de teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required]
        public string Direccion { get; set; }

        // Solo lectura, NO deben ser [Required]
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public DateTime Nacimiento { get; set; }
        public string NombreEPS { get; set; }
    }
}