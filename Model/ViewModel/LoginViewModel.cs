using System.ComponentModel.DataAnnotations;

namespace Proyecto.Model.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El documento es obligatorio")]
        [StringLength(30, ErrorMessage = "Documento demasiado largo")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}