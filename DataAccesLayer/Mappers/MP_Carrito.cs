using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccesLayer.Mappers
{
    public class MP_Carrito
    {
        private readonly Conexion cs = new Conexion();

        public int CrearCarrito(string ipCliente)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Ip_Cliente", ipCliente)
            };
            return cs.EscribirConRetorno("CrearCarrito", parametros);
        }

        public Carrito ObtenerCarritoPorIp(string ipCliente)
        {
            string storeProc = "ObtenerCarritoPorIp";

            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Ip_Cliente", ipCliente)
            };

            DataTable dt = cs.Leer(storeProc, parametros);

            if (dt.Rows.Count == 0)
            {
                return null; // No hay carrito para la IP dada
            }

            // Crear una instancia del carrito
            var carrito = new Carrito
            {
                Id_Carrito = Convert.ToInt32(dt.Rows[0]["Id_Carrito"]),
                Ip_Cliente = dt.Rows[0]["Ip_Cliente"].ToString(),
                Subtotal = Convert.ToDecimal(dt.Rows[0]["Subtotal"]),
                Total = Convert.ToDecimal(dt.Rows[0]["Total"]),

                lista = new List<Productos>()
            };

            // Llenar la lista de productos
            foreach (DataRow row in dt.Rows)
            {
                var producto = new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"]),

                    Descripcion = row["Descripcion"].ToString(),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    Cod_Producto = row["Cod_Producto"].ToString(),
                    Imagen = row["Imagen"].ToString(),
                    cant = Convert.ToInt32(row["Cantidad"].ToString())
                };

                carrito.lista.Add(producto);
            }

            return carrito;
        }


        public int AgregarProductos(int id_producto, string Ip_Cliente)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
               new SqlParameter("@Id_Producto", id_producto),
               new SqlParameter("@Ip_Cliente", Ip_Cliente),
            };
            return cs.Escribir("AgregarProductoConCarrito", parametros);

        }


        public int EliminarProducto(int id_carrito, int id_prod)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
               new SqlParameter("@Id_Carrito", id_carrito),
               new SqlParameter("@Id_Producto", id_prod),
            };
            return cs.Escribir("EliminarProductoDelCarrito", parametros);

        }


        public int FinalizarCompra(CarritoModel carrito)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Id_Carrito", carrito.Id_Carrito),
        new SqlParameter("@Nombre", carrito.Nombre ?? (object)DBNull.Value),
        new SqlParameter("@Apellido", carrito.Apellido ?? (object)DBNull.Value),
        new SqlParameter("@Mail", carrito.Mail ?? (object)DBNull.Value),
        new SqlParameter("@Telefono", carrito.Telefono ?? (object)DBNull.Value),
        new SqlParameter("@FormaPago", carrito.FormaPago ?? (object)DBNull.Value),
        new SqlParameter("@FormaEntrega", carrito.FormaEntrega ?? (object)DBNull.Value)
            };

            return cs.Escribir("FinalizarCompra", parametros);
        }


        public int CantProductos(string Ip_Cliente)
        {
            string storeProc = "ProductosCarrito";
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Ip_Cliente", Ip_Cliente),
            };

            object resultado = cs.ObetenerDatos("sp_ContarProductosCarrito", parametros);

            return resultado != null ? Convert.ToInt32(resultado) : 0; //


        }
    }
}
