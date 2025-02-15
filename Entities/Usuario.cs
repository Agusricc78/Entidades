using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Usuario
    {
       
            public int Id_Usuario { get; set; } // Clave primaria
            public string Nombre { get; set; }  // Nombre del usuario
            public string Apellido { get; set; } // Apellido del usuario
            public string Telefono { get; set; }   // Teléfono (puede ser nulo)
            public string Correo { get; set; }   // Correo del usuario
            public bool Activo { get; set; }     // Estado del usuario (activo/inactivo)
            public string Password { get; set; } // Contraseña del usuario
        
            public string Tipo { get; set; } 
            public string confirmarClave { get; set; }
            public bool Restablecer { get; set; }
    }
}
