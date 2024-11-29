using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DataAccess
    {
        private readonly AccesCore _objAccessCore;

        public DataAccess()
        {
            _objAccessCore = new AccesCore("CostaAzul");
        }

        public DataAccess(string connName)
        {
            _objAccessCore = new AccesCore(connName);
        }

        public List<T> GetList<T>(string sp, params object[] parameters) where T : class, new()
        {
            //utilizo la extension de datatable descripta en la class Helper.cs DataTableToList
            return _objAccessCore.GetList(sp, parameters).DataTableToList<T>();
        }

        public List<T> GetList<T>(string sp, T entity) where T : class, new()
        {
            //utilizo la extension de datatable descripta en la class Helper.cs DataTableToList
            return _objAccessCore.GetList(sp, entity).DataTableToList<T>();
        }

        public T Get<T>(string sp, T entity) where T : class, new()
        {
            return _objAccessCore.GetSingleObject<T>(sp, entity);

        }

        public T GetById<T>(string sp, params object[] parameters) where T : class, new()
        {
            return _objAccessCore.GetSingleObject<T>(sp, parameters);
        }

        public bool Insert(string sp, params object[] parameters)
        {
            return _objAccessCore.ExecuteNonQuery(sp, parameters);
        }

        public T Insert<T>(string sp, T obj) where T : class
        {
            _objAccessCore.ExecuteNonQuery(sp, obj);
            return obj;
        }

        /// <summary>
        /// Invoca un stored procedure de insersión de datos
        /// y retorna el último valor de identidad generado en una tabla en la sesión actual y ámbito actual
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int InsertIdentity(string sp, params object[] parameters)
        {
            return (int)_objAccessCore.ExecuteScalar<decimal>(sp, parameters);
        }

        public int InsertIdentity<T>(string sp, T obj) where T : class
        {
            return _objAccessCore.ExecuteScalar(sp, obj);
        }

        public bool Execute(string sp, params object[] parameters)
        {
            return _objAccessCore.ExecuteNonQuery(sp, parameters);
        }

        public bool Execute<T>(string sp, T obj) where T : class
        {
            return _objAccessCore.ExecuteNonQuery(sp, obj);
        }

        public int ExecuteInteger(string sp, params object[] parameters)
        {
            return _objAccessCore.ExecuteScalar<int>(sp, parameters);
        }

        public int ExecuteInteger<T>(string sp, T obj) where T : class
        {
            return _objAccessCore.ExecuteScalar(sp, obj);
        }

    }
}
