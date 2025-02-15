using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {
        private readonly SqlConnection conector;
        SqlCommand cmd = new SqlCommand();

        public Conexion()
        {
            string cadena = "Server=NICOlAS;Database=CostaAzul;Integrated Security=True;TrustServerCertificate=True;";
            conector = new SqlConnection(cadena);
            cmd = new SqlCommand();
        }
        public void Conectar()
        {
            conector.Open();
        }
        public void Desconectar()
        {
            conector.Close();
        }
        public DataTable Leer(string st, SqlParameter[] parametros = null)
        {

            DataTable tabla = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter(st, conector);


            adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (parametros != null)
            {
                adaptador.SelectCommand.Parameters.AddRange(parametros);
            }

            adaptador.Fill(tabla);
            return tabla;
        }



        public bool Verificar(string Procedure, SqlParameter[] param = null)
        {
            Conectar();

            SqlCommand cmd = new SqlCommand(Procedure, conector);
            cmd.CommandType = CommandType.StoredProcedure;
            if (param != null) cmd.Parameters.AddRange(param);

            object resultado = cmd.ExecuteNonQuery();

            Desconectar();

            return resultado != null && Convert.ToBoolean(resultado);

        }






        public bool ObtenerEstadoBloqueo(string username)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@UserName", username)
            };

            DataTable resultado = Leer("VerificarEstadoBloqueo", parametros);

            if (resultado.Rows.Count > 0)
            {
                return Convert.ToBoolean(resultado.Rows[0]["Bloqueo"]);
            }

            return false;
        }


        public int Escribir(string storeProc, SqlParameter[] parametros)
        {
            int r = 0;
            //transaccion
            Conectar();

            cmd = new SqlCommand(storeProc, conector);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parametros);

            try
            {
                r = cmd.ExecuteNonQuery();
                return r;

            }
            catch
            {
                return -1;
            }
            finally
            {
                Desconectar();
            }
        }

        SqlTransaction TR;

        void AceptarTX()
        {
            TR.Commit();
        }

        void CancelarTX()
        {
            TR.Rollback();
        }

        internal void AsignarID(string storeProc, object Entity)
        {
            Conectar();
            cmd = new SqlCommand(storeProc, conector);
            cmd.CommandType = CommandType.StoredProcedure;

            PropertyInfo Propntity = Entity.GetType().GetProperties()[0];
            Propntity.SetValue(Entity, cmd.ExecuteScalar());

            Desconectar();
        }

        public object ObetenerDatos(string storeProc, SqlParameter[] parametros = null)
        {
            Conectar();

            SqlCommand cmd = new SqlCommand(storeProc, conector);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parametros != null) cmd.Parameters.AddRange(parametros);

            object Resultado = cmd.ExecuteScalar();

            Desconectar();

            return Resultado;
        }

        public int EscribirConRetorno(string storeProc, SqlParameter[] parametros)
        {
            int resultado = 0;

            // Conectar a la base de datos
            Conectar();

            cmd = new SqlCommand(storeProc, conector);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parametros);

            // Iniciar la transacción
            TR = conector.BeginTransaction();
            cmd.Transaction = TR;

            try
            {
                // Ejecutar el comando y capturar el valor devuelto por el procedimiento
                var returnParameter = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(returnParameter);

                cmd.ExecuteNonQuery();

                // Confirmar la transacción
                AceptarTX();

                // Obtener el valor devuelto
                resultado = (int)returnParameter.Value;
            }
            catch
            {
                // Revertir la transacción en caso de error
                CancelarTX();
                resultado = -1;
            }
            finally
            {
                // Desconectar de la base de datos
                Desconectar();
            }

            return resultado;
        }


    }
}
