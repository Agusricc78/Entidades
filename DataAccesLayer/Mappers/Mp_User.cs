using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;

namespace DataAccesLayer.Mappers
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

        public int RegistrarUsuario(string nom, int? tel, string correo, string contra)
        {


            SqlParameter[] sp = new SqlParameter[]
            {
             new SqlParameter("@Nombre",nom),
             new SqlParameter("@Telefono",tel),
             new SqlParameter("@Correo",correo),
             new SqlParameter("@Password",contra)
             };

            return cn.Escribir("RegistrarUser", sp);
        }



        public Usuario ObtenerUsuarioXNom(string Nombre)
        {
            Usuario user = new Usuario();
            SqlParameter[] parametros = new SqlParameter[]
               {
                   new SqlParameter("@Nombre", Nombre)

               };
            DataTable dt = cn.Leer("ObtenerUserxNombre", parametros);
            if (dt.Rows.Count > 0)
            {
                DataRow fila = dt.Rows[0];

                // Mapea los datos del DataRow a un objeto Usuario
                Usuario usuario = new Usuario
                {
                    Id_Usuario = Convert.ToInt32(fila["Id_Usuario"]),
                    Nombre = Convert.ToString(fila["Nombre"]),
                    Password = Convert.ToString(fila["Password"]),
                    Activo = Convert.ToBoolean(fila["Activo"]),
                    Correo = Convert.ToString(fila["Correo"]),
                    //Telefono = Convert.ToInt32(fila["Telefono"]),
                    Apellido = Convert.ToString(fila["Apellido"]),

                };

                return usuario;
            }
            else
            {
                return null;
            }

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
