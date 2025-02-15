using CapaDatos.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CapaNegocio.BLL
{
    public class BLL_Carrito
    {
        MP_Carrito obj = new MP_Carrito();

        public int ExisteCarrito(string ipcliente,int idproducto, out int cantidad)
        {
            return obj.ExisteCarrito(ipcliente, idproducto,out cantidad);
        }

        public int OperacionCarrito(int idcliente, int idproducto,string ipcliente, bool sumar, out string Mensaje)
        {
                return obj.OperacionCarrito(idcliente,idproducto,ipcliente,sumar,out Mensaje);
        }

        public object CantidadEnCarrito(string ipcliente)
        {
            return obj.CantidadEnCarrito(ipcliente);
        }

        public DataTable GetCarrito(string ipcliente)
        {
            return obj.GetCarrito(ipcliente);
        }

        public int EliminarCarrito(string ipcliente,int idproducto)
        {
            return obj.EliminarCarrito(ipcliente,idproducto);
        }
    }
}
