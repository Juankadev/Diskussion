using System.ComponentModel.DataAnnotations;

namespace Diskussion.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Password { get; set; }
    }
}
