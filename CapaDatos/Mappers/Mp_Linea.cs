using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class Mp_Linea
    {
        private readonly Conexion cn = new Conexion();
        public int RegistrarLinea(Linea ln, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializamos por si no se asigna valor.


            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] sp = new SqlParameter[]
            {
            new SqlParameter("@Nombre", ln.Nombre),
            new SqlParameter("@Activo", ln.Activo),
            parametroMensaje,
            parametroResultado
            };

            // Ejecutar la consulta.
            cn.Escribir("RegistrarLinea", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int UpdateLinea(Linea ln, out string Mensaje)
        {
            Mensaje = string.Empty;


            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };


            SqlParameter[] sp = new SqlParameter[]
            {
             new SqlParameter("@Id_Linea",ln.Id_Linea),
             new SqlParameter("@Nombre",ln.Nombre),
             new SqlParameter("@Activo",ln.Activo),
             parametroMensaje,
             parametroResultado
             };

            cn.Escribir("UpdateLinea", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public int EliminarLinea(int id, out string Mensaje)
        {
            Mensaje = string.Empty;


            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };


            SqlParameter[] sp = new SqlParameter[]
            {
             new SqlParameter("@Id_Linea",id),
             parametroMensaje,
             parametroResultado
             };

            cn.Escribir("EliminarLinea", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public DataTable ListarLinea()
        {
            return cn.Leer("ListarLineas");
        }

        public DataTable GetLineaCategoria(int idcategoria)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@Id_Categoria",idcategoria),
            };

            return cn.Leer("ListarLineaCategoria", sp);
        }
    }
}
