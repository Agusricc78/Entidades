using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class Mp_Productos
    {
        private readonly Conexion cn = new Conexion();

        public int AgregarProducto(Productos pro)
        {
            PropertyInfo[] Propsentity = pro.GetType().GetProperties();
            List<SqlParameter> ListPara = new List<SqlParameter>();

            foreach (PropertyInfo pi in Propsentity)
            {
                string name = pi.Name;
                object valor = pi.GetValue(pro);

                SqlParameter parametros = new SqlParameter($"@{name}", valor);

                ListPara.Add(parametros);
            }

            return cn.Escribir("AgregarProducto", ListPara.ToArray());
        }

        public DataTable ListarProductos()
        {
            return cn.Leer("ListarProductos");
        }

        public int EliminarProducto(int id)
        {
            string storeProc = "EliminarProductoxId";


            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@Id_Producto",id)
            };


            return cn.Escribir(storeProc, parametros);
        }





    }
}
