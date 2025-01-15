using Entities;
using System.ComponentModel.DataAnnotations;

namespace CostaAzulWeb.Models
{
    public class CarritoViewModel
    {
        public CarritoViewModel()
        {
            Productos = new List<Productos>();
        }

        public int Id_Carrito { get; set; } // Identificador único del carrito
        [Required(ErrorMessage = "La Ip del cliente es Requerida.")]
        public string Ip_Cliente { get; set; } // Dirección IP asociada al carrito
        public decimal Subtotal { get; set; } // Subtotal acumulado del carrito
        public decimal Total { get; set; } // Total acumulado
        public List<Productos> Productos { get; set; } // Lista de productos en el carrito

        public string Direccion { get; set; }
        public string? Nombre { get; set; } // Nombre del cliente
        public string? Apellido { get; set; } // Apellido del cliente
        public string? Mail { get; set; } // Correo electrónico
        public string? Telefono { get; set; } // Teléfono
        public string? FormaPago { get; set; } // Forma de pago
        public string? FormaEntrega { get; set; }





    }
}
