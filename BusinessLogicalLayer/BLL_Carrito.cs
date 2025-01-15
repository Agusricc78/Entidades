using DataAccesLayer.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class BLL_Carrito
    {
        MP_Carrito mp = new MP_Carrito();

        public int CrearCarrito(string ipCliente)
        {
            return mp.CrearCarrito(ipCliente);  

        }

        public int AgregarProductoCarrito(int id_producto, string ip_cliente)
        {
            return mp.AgregarProductos(id_producto,ip_cliente);
        }

        public int EliminarProductoCarrito(int idproducto, int id_carrito)
        {
            return mp.EliminarProducto(id_carrito, idproducto);
        }

        public Carrito VerCarrito(string Ip_Cliente)
        {
            return mp.ObtenerCarritoPorIp(Ip_Cliente);

        }

        public int Finalizar(CarritoModel cm)
        {
            return mp.FinalizarCompra(cm);
        }

        public int CantProductos(string Ip_Cliente)
        {
            return mp.CantProductos(Ip_Cliente);
        }



    }
}
