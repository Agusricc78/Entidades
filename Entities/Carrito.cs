using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Carrito
    {
        public int Id_Carrito { get; set; }
        public string Ip_Cliente { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuentos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int Cantidad { get; set; }   

        public List<Productos> lista { get; set; }


    }
}
