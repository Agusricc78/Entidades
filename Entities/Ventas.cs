using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Ventas
    {
        public int Id_Venta { get; set; }
        public int Id_Cliente { get; set; }
        public decimal MontoTotal { get; set; }
        public string Direccion { get; set; }
        public string Id_Transaccion { get; set; }
        public string FormaPago { get; set; }
        public string FormaRetiro { get; set; }
        public int Id_Estado { get; set; }
        public string NroPedido { get; set; }
        public string Ip_Cliente { get; set; }
        public int TotalProductos { get; set; }
        public int Id_Localidad { get; set; }
      
    }
}
