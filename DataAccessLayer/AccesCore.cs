using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccessLayer
{
    internal class AccesCore
    {
        public AccesCore(string connName)
        {
            // Busca la cadena de conexión en el archivo Web.config
            var connStringSetting = ConfigurationManager.ConnectionStrings[connName];
            if (connStringSetting == null)
            {
                throw new InvalidOperationException($"No se encontró la cadena de conexión '{connName}' en el archivo de configuración.");
            }
            _connectionString = connStringSetting.ConnectionString;
        }


        #region Propiedades

        private readonly string _connectionString = "DataSource = PcAgus;database = CostaAzul;Integrates Security = true";

        #endregion

        #region MetodosDb

        public DataTable GetList(string sp, params object[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);

                SqlCommandBuilder.DeriveParameters(sqlComm);
                var cParameters = 0;
                if (parameters != null) cParameters = parameters.Length;

                if (cParameters == sqlComm.Parameters.Count - 1)
                {
                    for (var i = 1; i <= sqlComm.Parameters.Count - 1; i++)
                    {
                        sqlComm.Parameters[i].Value = parameters[i - 1];
                    }
                }

                var dt = new DataTable();
                dt.Load(sqlComm.ExecuteReader());
                return dt;
            }
        }

        private static SqlCommand Execute(string sp, SqlConnection connection)
        {
            var sqlComm = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = sp
            };
            connection.Open();
            return sqlComm;
        }

        public T GetSingleObject<T>(string sp, params object[] parameters) where T : new()
        {
            var retVal = new T();

            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);

                SqlCommandBuilder.DeriveParameters(sqlComm);
                var cParameters = 0;
                if (parameters != null) cParameters = parameters.Length;

                if (cParameters == sqlComm.Parameters.Count - 1)
                {
                    for (var i = 1; i <= sqlComm.Parameters.Count - 1; i++)
                    {
                        sqlComm.Parameters[i].Value = parameters[i - 1];
                    }
                }

                var dt = new DataTable();
                dt.Load(sqlComm.ExecuteReader());
                if (dt.Rows.Count > 0) retVal = dt.Rows[0].DataRowToObject<T>();
                connection.Close();
                return retVal;
            }
        }

        public T GetSingleObject<T>(string sp, T entity) where T : class, new()
        {
            var retVal = new T();
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);
                SqlCommandBuilder.DeriveParameters(sqlComm);
                entity.ObjectToSqlParams(sqlComm.Parameters);

                var dt = new DataTable();
                dt.Load(sqlComm.ExecuteReader());
                if (dt.Rows.Count > 0) retVal = dt.Rows[0].DataRowToObject<T>();
                connection.Close();
                return retVal;
            }
        }

        public bool ExecuteNonQuery(string sp, params object[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);

                SqlCommandBuilder.DeriveParameters(sqlComm);
                var cParameters = 0;
                if (parameters != null) cParameters = parameters.Length;

                if (cParameters == sqlComm.Parameters.Count - 1)
                {
                    for (var i = 1; i <= sqlComm.Parameters.Count - 1; i++)
                    {
                        sqlComm.Parameters[i].Value = parameters[i - 1];
                    }
                }

                sqlComm.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }


        public DataTable GetList<T>(string sp, T entity) where T : class
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);
                SqlCommandBuilder.DeriveParameters(sqlComm);
                entity.ObjectToSqlParams(sqlComm.Parameters);

                var dt = new DataTable();
                dt.Load(sqlComm.ExecuteReader());
                return dt;
            }
        }



        /// <summary>
        ///     ejecuta el stored procedure pasado en el parámetro sp
        ///     los parámetros de entrada del sp serán mapeados de T
        ///     es importante que los parámetros esperados por el sp
        ///     existan en T y sean de tipo equivalente, caso contrario
        ///     serán enviados como dbnull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery<T>(string sp, T entity) where T : class
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);

                SqlCommandBuilder.DeriveParameters(sqlComm);

                entity.ObjectToSqlParams(sqlComm.Parameters);

                sqlComm.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }

        public int ExecuteScalar<T>(string sp, T entity) where T : class
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);
                SqlCommandBuilder.DeriveParameters(sqlComm);

                entity.ObjectToSqlParams(sqlComm.Parameters);

                var retVal = (int)(decimal)sqlComm.ExecuteScalar();
                connection.Close();
                return retVal;
            }
        }

        public T ExecuteScalar<T>(string sp, params object[] parameters) where T : struct
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlComm = Execute(sp, connection);

                SqlCommandBuilder.DeriveParameters(sqlComm);
                var cParameters = 0;
                if (parameters != null) cParameters = parameters.Length;

                if (cParameters == sqlComm.Parameters.Count - 1)
                {
                    for (var i = 1; i <= sqlComm.Parameters.Count - 1; i++)
                    {
                        sqlComm.Parameters[i].Value = parameters[i - 1];
                    }
                }

                var retVal = (T)sqlComm.ExecuteScalar();
                connection.Close();
                return retVal;
            }
        }

        #endregion

    }

}

