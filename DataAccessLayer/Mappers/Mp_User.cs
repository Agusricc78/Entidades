using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccessLayer
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

        public int RegistrarUsuario(Usuario us)
        {

            
            SqlParameter[] sp = new SqlParameter[]
            {
             
             new SqlParameter("@Apellido",us.Apellido),

             };

            return cn.Escribir("RegistrarUser", sp);
        }

        public bool ValidarUsuario(string nombre)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                   new SqlParameter("@Nombre", nombre),
                   new SqlParameter
                   {
                 ParameterName = "@Exists",
                 SqlDbType = SqlDbType.Bit,
                 Direction = ParameterDirection.Output
                   }
            };
            cn.Verificar("ValidarUsuario", parametros);

            bool exists = (bool)parametros[1].Value;
            return exists;
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
                    Telefono= Convert.ToInt32(fila["Telefono"]),
                    Apellido = Convert.ToString(fila["Apellido"]),

                };
                
                return usuario;
            }
            else
            {
                return null;
            }

        }






    }
}
