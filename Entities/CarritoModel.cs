using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities
{
    public class CarritoModel
    {
        public CarritoModel()
        {
        }

        public int Id_Carrito { get; set; } // Identificador único del carrito
       
        public string Ip_Cliente { get; set; } // Dirección IP asociada al carrito
        public decimal Subtotal { get; set; } // Subtotal acumulado del carrito
        public decimal Total { get; set; } // Total acumulado
        public List<Productos> Productos { get; set; } // Lista de productos en el carrito
        public string Estado { get; set; }

        public string Nombre { get; set; } // Nombre del cliente
        public string Apellido { get; set; } // Apellido del cliente
        public string Mail { get; set; } // Correo electrónico
        public string Telefono { get; set; } // Teléfono
        public string FormaPago { get; set; } // Forma de pago
        public string FormaEntrega { get; set; }


    }
}
