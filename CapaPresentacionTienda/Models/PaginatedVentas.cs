using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacionTienda.Models
{
    public class PaginatedVentas
    {
        public string NroPedido { get; set; }
        public string Id_Transaccion { get; set; }
        public List<VentasViewModel> Productos { get; set; }
    }
}
