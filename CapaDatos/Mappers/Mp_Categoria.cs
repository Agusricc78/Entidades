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
    public class Mp_Categoria
    {
        private readonly Conexion cn = new Conexion();
        public int RegistrarCategoria(Categorias ct, out string Mensaje)
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
            new SqlParameter("@Nombre", ct.Nombre),
            new SqlParameter("@Activo", ct.Activo),
            parametroMensaje,
            parametroResultado
            };

            // Ejecutar la consulta
            cn.Escribir("RegistrarCategoria", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int UpdateCategoria(Categorias ct, out string Mensaje)
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
             new SqlParameter("@Id_Categoria",ct.Id_Categoria),
             new SqlParameter("@Nombre",ct.Nombre),
             new SqlParameter("@Activo",ct.Activo),
             parametroMensaje,
             parametroResultado
             };

            cn.Escribir("UpdateCategoria", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public int EliminarCategoria(int id, out string Mensaje)
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
             new SqlParameter("@Id_Categoria",id),
             parametroMensaje,
             parametroResultado
             };

             cn.Escribir("EliminarCategoria", sp);
            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public DataTable ListarCategorias()
        {
            return cn.Leer("ListarCategorias");
        }
    }
}
