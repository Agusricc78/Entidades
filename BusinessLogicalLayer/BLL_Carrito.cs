using DataAccesLayer.Mappers;
using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
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

        public int AgregarProductoCarrito(int id_producto, string ip_cliente, int cant)
        {
            return mp.AgregarProductos(id_producto,ip_cliente, cant);
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

       public List<CarritoModel> ObtenerCarritos(string estado)
        {
            return mp.ObtenerCarritosPorEstado(estado);
        }


        public List<CarritoModel> GetCarritos()
        {
            return mp.GetCarritos();
        }

        public int ActualizarCant(int idcarrito,int id,int cant)
        {
            return mp.ActualizarCant(idcarrito, id, cant);
        }

    }
}
