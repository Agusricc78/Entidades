using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Mappers
{
    public class Mp_Productos
    {
        private readonly DataAccesLayer.Conexion cn = new DataAccesLayer.Conexion();

        public int AgregarProducto(Productos pro)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
             new SqlParameter("@Nombre",pro.Nombre),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Imagen",pro.Imagen),

            };
            return cn.Escribir("AgregarProducto", parametros);
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
            new SqlParameter("@Cod_Producto",id)
            };


            return cn.Escribir(storeProc, parametros);
        }

        public bool VerificarExistencia(string nom, int codigo)
        {
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@Nombre",nom ),
            new SqlParameter("@Cod_Producto", codigo),
            new SqlParameter
         {
             ParameterName = "@Exists",
             SqlDbType = SqlDbType.Bit,
             Direction = ParameterDirection.Output
         }

         };

            cn.Leer("sp_VerificarProducto", param);

            bool exists = (bool)param[2].Value;
            return exists;
        }

        public Productos ObtenerProducto(int cod)
        {
            
            SqlParameter[] parametros = new SqlParameter[]
               {
                   new SqlParameter("@Cod_Producto", cod)

               };
            DataTable dt = cn.Leer("ObtenerProductoXCod", parametros);
            if (dt.Rows.Count > 0)
            {
                DataRow fila = dt.Rows[0];

                // Mapea los datos del DataRow a un objeto Usuario
                Productos pro = new Productos
                {
                    Id_Producto = Convert.ToInt32(fila["Id_Producto"]),
                    Nombre = Convert.ToString(fila["Nombre"]),
                    Cod_Producto = Convert.ToInt32(fila["Cod_Producto"]),
                    Activo = Convert.ToBoolean(fila["Activo"]),
                    Id_Categoria = Convert.ToString(fila["Id_Categoria"]),
                    Precio = Convert.ToInt32(fila["Precio"]),
                    stock = Convert.ToInt32(fila["Stock"]),
                    Descripcion = Convert.ToString(fila["Descripcion"]),
                    Imagen = Convert.ToString(fila["Imagen"]),
                    

                };
                return pro;


            }
            else
            {
                return null;
            }
        }


        public int EditarPro(Productos pro)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("Id_Producto",pro.Id_Producto),
             new SqlParameter("@Nombre",pro.Nombre),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Imagen",pro.Imagen),

            };
            return cn.Escribir("EditarProducto", parametros);
        }





    }
}
