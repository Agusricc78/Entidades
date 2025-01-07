using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Ventas
    {
        public int Id_Venta { get; set; }
        public int Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public int MontoTotal { get; set; }
        public DateTime FechaVenta { get; set; }    
        public int Id_Transaccion { get; set; } 





    }
}
