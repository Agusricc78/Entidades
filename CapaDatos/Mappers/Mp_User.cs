using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Net.Http.Headers;
using System.Globalization;

namespace CapaDatos.Mappers
{
    public class Mp_User
    {
        private readonly Conexion cn = new Conexion();



        public DataTable autenticar(string username, string passw)
        {


            SqlParameter[] parametros = new SqlParameter[]
                {
                   new SqlParameter("@Nombre", username),
                   new SqlParameter("@Password", passw)
                };

            return cn.Leer("sp_GetUserByUsernameAndPassword", parametros);
        }

        public int RegistrarUsuario(Usuario us, out string Mensaje)
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
            new SqlParameter("@Nombre", us.Nombre),
            new SqlParameter("@Apellido", us.Apellido),
            new SqlParameter("@Telefono", us.Telefono),
            new SqlParameter("@Correo", us.Correo),
            new SqlParameter("@Password", us.Password),
            new SqlParameter("@Tipo", us.Tipo),
            new SqlParameter("@Activo", us.Activo),
            new SqlParameter("@Restablecer",us.Restablecer),
            parametroMensaje,
            parametroResultado
            };

            // Ejecutar la consulta.
            cn.Escribir("RegistrarUser", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int RegistrarClientes(Usuario us, out string Mensaje)
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
            new SqlParameter("@Nombre", us.Nombre),
            new SqlParameter("@Apellido", us.Apellido),
            new SqlParameter("@Telefono", us.Telefono),
            new SqlParameter("@Correo", us.Correo),
            new SqlParameter("@Password", us.Password),
            new SqlParameter("@Tipo", us.Tipo),
            new SqlParameter("@Activo", us.Activo),
            new SqlParameter("@Restablecer",us.Restablecer),
            parametroMensaje,
            parametroResultado
            };

            // Ejecutar la consulta.
            cn.Escribir("RegistrarUser", sp);

            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public int CambiarClave(int id,string nuevaPassword, out string Mensaje)
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
                new SqlParameter("@Id_Usuario",id),
                new SqlParameter("@Password",nuevaPassword),
                parametroMensaje,
                parametroResultado
            };

            cn.Escribir("UpdateClave", sp);

            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int RestablecerClave(int id, string Password, out string Mensaje)
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
                new SqlParameter("@Id_Usuario",id),
                new SqlParameter("@Password",Password),
                parametroMensaje,
                parametroResultado
            };

            cn.Escribir("RestablecerClave", sp);

            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

        public int UpdateUsuario(Usuario us, out string Mensaje)
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
             new SqlParameter("@Id_Usuario",us.Id_Usuario),
             new SqlParameter("@Nombre",us.Nombre),
             new SqlParameter("@Apellido",us.Apellido),
             new SqlParameter("@Telefono",us.Telefono),
             new SqlParameter("@Correo",us.Correo),
             new SqlParameter("@Tipo",us.Tipo),
             new SqlParameter("@Activo",us.Activo),
             parametroMensaje,
             parametroResultado
             };

            cn.Escribir("UpdateUser", sp);
            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public bool VerificarExistencia(string nom, string correo)
        {
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@Nombre",nom ),
            new SqlParameter("@Correo", correo),
            new SqlParameter
         {
             ParameterName = "@Exists",
             SqlDbType = SqlDbType.Bit,
             Direction = ParameterDirection.Output
         }

         };

            cn.Leer("sp_VerificarUsuarioExistente", param);

            bool exists = (bool)param[2].Value;
            return exists;
        }

        public DataTable GetAllUsers()
        {
            return cn.Leer("GetAllUsers");
        }

    }
}
