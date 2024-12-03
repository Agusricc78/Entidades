using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   
        public class Conexion
        {
            private readonly SqlConnection conector = ConexionSingleton.ObtenerInstancia();
            SqlCommand cmd = new SqlCommand();


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

                TR = conector.BeginTransaction();
                cmd.Transaction = TR;

                try
                {
                    r = cmd.ExecuteNonQuery();
                    //agregar el commit()
                    AceptarTX();
                    return r;

                }
                catch
                {
                    //rollback()
                    CancelarTX();
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

        }
    }

