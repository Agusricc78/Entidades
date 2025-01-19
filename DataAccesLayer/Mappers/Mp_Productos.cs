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
             new SqlParameter("@Id_Linea",pro.Id_Linea),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Imagen",pro.Imagen),
             new SqlParameter("@Id_Catalogo",pro.Id_Catalogo),
            };
            return cn.Escribir("AgregarProducto", parametros);
        }

        public DataTable ListarProductos()
        {
            return cn.Leer("ListarProductos");
        }

        public int EliminarProducto(string id)
        {
            string storeProc = "EliminarProductoxId";


            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@Cod_Producto",id)
            };


            return cn.Escribir(storeProc, parametros);
        }

        public bool VerificarExistencia(string codigo)
        {
            SqlParameter[] param = new SqlParameter[]
            {
            
            new SqlParameter("@Cod_Producto", codigo),
            new SqlParameter
         {
             ParameterName = "@Exists",
             SqlDbType = SqlDbType.Bit,
             Direction = ParameterDirection.Output
         }

         };

            cn.Leer("sp_VerificarProducto", param);

            bool exists = (bool)param[1].Value;
            return exists;
        }

        public Productos ObtenerProducto(string cod)
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
                    Id_Linea = Convert.ToInt32(fila["Id_Linea"]),
                    Cod_Producto = Convert.ToString(fila["Cod_Producto"]),
                    Activo = Convert.ToBoolean(fila["Activo"]),
                    Id_Categoria = Convert.ToString(fila["Id_Categoria"]),
                    Precio = Convert.ToInt32(fila["Precio"]),
                    stock = Convert.ToInt32(fila["Stock"]),
                    Descripcion = Convert.ToString(fila["Descripcion"]),
                    Imagen = Convert.ToString(fila["Imagen"]),
                    Id_Catalogo = Convert.ToInt32(fila["Id_Producto"]),

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
             new SqlParameter("@Id_Linea",pro.Id_Linea),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Imagen",pro.Imagen),
             new SqlParameter("@Id_Catalogo",pro.Id_Catalogo)

            };
            return cn.Escribir("EditarProducto", parametros);
        }


        public DataTable ListarCatalogos(int id)
        {
            string storeProc = "ListarProductosXCatalogo";


            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@Id_Catalogo",id)
            };
            return cn.Leer(storeProc, parametros);
        }


        public DataTable FiltrarProductos(int? categoriaId, int? lineaId, string codigo)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Id_Categoria", categoriaId ?? (object)DBNull.Value),
        new SqlParameter("@Id_Linea", lineaId ?? (object)DBNull.Value),
        new SqlParameter("@Cod_Producto", string.IsNullOrEmpty(codigo) ? (object)DBNull.Value : codigo)
            };

            return cn.Leer("FiltrarProductos", parametros);
        }

        public List<Productos> ObtenerProductosMasVendidos()
        {
            string storedProcedure = "sp_ProductosMasVendidos";
            DataTable dt = cn.Leer(storedProcedure); // Asumiendo que cs es tu conexión de base de datos.

            var productos = new List<Productos>();

            foreach (DataRow row in dt.Rows)
            {
                productos.Add(new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"]),
                    Descripcion = row["NombreProducto"].ToString(),
                    Cod_Producto = row["Cod_Producto"].ToString(),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    Imagen = row["Imagen"].ToString(),
                   
                });
            }

            return productos;
        }


    }
}
