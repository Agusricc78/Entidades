using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacionTienda.Models
{
    public class VentasViewModel
    {
        public string NroPedido { get; set; }
        public string Id_Transaccion { get; set; }
        public string Imagen { get; set; }
        public string ExtImagen { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}