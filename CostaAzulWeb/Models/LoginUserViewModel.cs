using System.ComponentModel.DataAnnotations;

namespace CostaAzulWeb.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



    }
}
