using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CostaAzulWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Campos no requeridos
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        public bool Activo { get; set; }
    }

}