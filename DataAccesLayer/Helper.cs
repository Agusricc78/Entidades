using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;

namespace DataAccessLayer
{
    public static class Helper
    {
        #region TypesDictionaries
        static Dictionary<SqlDbType, Type> SqlNetTypes = new Dictionary<SqlDbType, Type>
        {
               {SqlDbType.BigInt, typeof(Int64)},
               {SqlDbType.Binary, typeof(byte[])},
               {SqlDbType.Bit, typeof(Boolean)},
               {SqlDbType.Char, typeof(String)},
               {SqlDbType.Date, typeof(DateTime)},
               {SqlDbType.DateTime, typeof(DateTime)},
               {SqlDbType.DateTime2, typeof(DateTime)},
               {SqlDbType.DateTimeOffset, typeof(DateTimeOffset)},
               {SqlDbType.Decimal, typeof(decimal)},
               {SqlDbType.Float, typeof(double)},
               {SqlDbType.Image, typeof(byte[])},
               {SqlDbType.Int, typeof(Int32)},
               {SqlDbType.Money, typeof(decimal)},
               {SqlDbType.NChar, typeof(string)},
               {SqlDbType.NText, typeof(string)},
               {SqlDbType.NVarChar, typeof(string)},
               {SqlDbType.Real, typeof(Single)},
               {SqlDbType.SmallDateTime, typeof(DateTime)},
               {SqlDbType.SmallInt, typeof(Int16)},
               {SqlDbType.SmallMoney, typeof(decimal)},
               {SqlDbType.Text, typeof(string)},
               {SqlDbType.Time, typeof(TimeSpan)},
               {SqlDbType.Timestamp, typeof(byte[])},
               {SqlDbType.TinyInt, typeof(byte)},
               {SqlDbType.UniqueIdentifier, typeof(Guid)},
               {SqlDbType.VarBinary, typeof(byte[])},
               {SqlDbType.VarChar, typeof(string)},
               {SqlDbType.Variant, typeof(object)}

            };

        #endregion

        /// <summary>
        ///  if exists get the DBValue attribut value
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetDbValue(PropertyInfo property)
        {
            var retValue = property.Name;
            foreach (var attribData in property.GetCustomAttributesData())
            {
                if (attribData.ConstructorArguments.Count == 1)
                {
                    var typeName = attribData.Constructor.DeclaringType.Name;
                    if (typeName == "DbValueAttribute")
                    {
                        retValue = attribData.ConstructorArguments[0].Value.ToString();
                    }
                }
            }
            return retValue;
        }

        /// <summary>
        /// metodo extensor que mapea un dataTable y devuelve una list<T>
        /// es necesario que los nombres de las propiedades del object T
        /// coincidan exactamente con los nombres de campo a mapear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable dataTable) where T : new()
        {
            var retList = new List<T>();

            const BindingFlags publicFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            const BindingFlags nonPublicFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
            var objpublicNames = (from PropertyInfo aProp in typeof(T).GetProperties(publicFlags)
                                  select new
                                  {
                                      Name = GetDbValue(aProp).ToUpper(),
                                      Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                  }).ToList();


            var objFieldNames = (from PropertyInfo aProp in typeof(T).GetProperties(nonPublicFlags)
                                 select new
                                 {
                                     Name = GetDbValue(aProp).ToUpper(),
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                 }).ToList();

            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName.ToUpper(),
                                         Type = aHeader.DataType
                                     }).ToList();

            objFieldNames = objFieldNames.Union(objpublicNames).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (var dr in dataTable.AsEnumerable().ToList())
            {
                var retVal = new T();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = retVal.GetType().GetProperty(aField.Name, nonPublicFlags) ??
                                                 retVal.GetType().GetProperty(aField.Name, publicFlags);
                    if (propertyInfos != null)
                    {
                        var value = (dr[aField.Name] == DBNull.Value) ? null : dr[aField.Name];
                        propertyInfos.SetValue(retVal, value, null);
                    }
                }
                retList.Add(retVal);
            }
            return retList;
        }

        public static T DataRowToObject<T>(this DataRow dr) where T : new()
        {
            var retVal = new T();

            const BindingFlags publicFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            const BindingFlags nonPublicFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase;

            var objpublicNames = (from PropertyInfo aProp in typeof(T).GetProperties(publicFlags)
                                  select new
                                  {
                                      Name = GetDbValue(aProp).ToUpper(),
                                      Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                  }).ToList();


            var objFieldNames = (from PropertyInfo aProp in typeof(T).GetProperties(nonPublicFlags)
                                 select new
                                 {
                                     Name = GetDbValue(aProp).ToUpper(),
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                 }).ToList();

            var dataTblFieldNames = (from DataColumn aHeader in dr.Table.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName.ToUpper(),
                                         Type = aHeader.DataType
                                     }).ToList();

            objFieldNames = objFieldNames.Union(objpublicNames).ToList();

            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (var aField in commonFields)
            {
                PropertyInfo propertyInfos = retVal.GetType().GetProperty(aField.Name, nonPublicFlags) ??
                                                 retVal.GetType().GetProperty(aField.Name, publicFlags);
                if (propertyInfos != null)
                {
                    var value = (dr[aField.Name] == DBNull.Value) ? null : dr[aField.Name];
                    propertyInfos.SetValue(retVal, value, null);
                }

            }

            return retVal;
        }

        public static void ObjectToSqlParams<T>(this T obj, SqlParameterCollection sqlParams) where T : class
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase;


            foreach (SqlParameter aParam in sqlParams)
            {
                if (aParam.Direction != ParameterDirection.Input) continue;
                var propertyInfos = obj.GetType().GetProperty(aParam.ParameterName.Substring(2), flags);
                if (propertyInfos != null && SqlNetTypes[aParam.SqlDbType] == (Nullable.GetUnderlyingType(propertyInfos.PropertyType) ?? propertyInfos.PropertyType))
                {
                    aParam.Value = propertyInfos.GetValue(obj) ?? DBNull.Value;
                }
                else { aParam.Value = DBNull.Value; }

            }
        }

    



     }
}
