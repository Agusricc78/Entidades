using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class MP_Carrito
    {
        private readonly Conexion cn = new Conexion();
        public int ExisteCarrito(string ipcliente,int idproducto, out int cantidad)
        {
 
            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroCantidad = new SqlParameter("@Cantidad", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };


            SqlParameter[] sp = new SqlParameter[]
            {
            new SqlParameter("@Ip_Cliente",ipcliente),
            new SqlParameter("@IdProducto",idproducto),
            parametroResultado,
            parametroCantidad
            };


            cn.Escribir("ExisteCarrito", sp);

            cantidad = parametroCantidad.Value != DBNull.Value ? Convert.ToInt32(parametroCantidad.Value) : 0;

            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int OperacionCarrito(int idcliente,int idproducto,string ipcliente,bool sumar, out string Mensaje)
        {

            Mensaje = string.Empty; // Inicializamos por si no se asigna valor.

            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] sp = new SqlParameter[]
            {
            new SqlParameter("@Id_Cliente", idcliente),
            new SqlParameter("@Ip_Cliente",ipcliente),
            new SqlParameter("@Id_Producto", idproducto),
            new SqlParameter("@sumar",sumar),
            parametroMensaje,
            parametroResultado
            };

            // Ejecutar la consulta
             cn.Escribir("OperacionCarrito", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public object CantidadEnCarrito(string ipcliente)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
             new SqlParameter("@Ip_Cliente",ipcliente),
           
      
             };

            return cn.ObetenerDatos("CantidadEnCarrito", sp);
        }


        public DataTable GetCarrito(string ipcliente)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@Ip_Cliente",ipcliente)
            };

            return cn.Leer("ObtenerCarritoCliente", sp);

        }


        public int EliminarCarrito(string ipcliente,int idproducto)
        {

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] sp = new SqlParameter[]
            {
             new SqlParameter("@Ip_Cliente",ipcliente),
             new SqlParameter("@Id_Producto",idproducto),
             parametroResultado
             };

            cn.Escribir("EliminarCarrito", sp);
            // Obtener los valores de los parámetros de salida
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }
    }
}
