using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DetalleVenta
    {
        public int Id_DetalleVenta { get; set; }
        public Ventas Ventas { get; set; }
        public Productos Productos { get; set; }
        public int Cantidad {  get; set; }
        public decimal Total { get; set; }
    }
}
