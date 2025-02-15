using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer
{
   
        public class ConexionSingleton
        {

            private static SqlConnection instancia;

            private ConexionSingleton() { }

            public static SqlConnection ObtenerInstancia()
            {
                if (instancia == null)
                {

                    string cadenaConexion = "Server=NICOlAS; Database=CostaAzul; Integrated Security=True;TrustServerCertificate=True;";

                    instancia = new SqlConnection(cadenaConexion);
                }

                return instancia;
            }



        }
   
}
