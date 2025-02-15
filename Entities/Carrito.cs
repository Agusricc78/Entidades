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
        public string Id_Cliente { get; set; }
        public int Id_Producto { get; set; }
        public Productos producto { get; set; }
        public int Cantidad { get; set; }
    }
}
