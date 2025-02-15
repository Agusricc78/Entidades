using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class Mp_Productos
    {
        private readonly Conexion cn = new Conexion();

        public int AgregarProducto(Productos pro, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializamos por si no se asigna valor.

            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] parametros = new SqlParameter[]
            {
             new SqlParameter("@Id_Linea",pro.Id_Linea),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Id_Catalogo",pro.Id_Catalogo),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Electrico",pro.Electrico),
             new SqlParameter("@Nombre",pro.Nombre),
             new SqlParameter("@Imagen", SqlDbType.VarBinary) { Value = (object)pro.Imagen ?? DBNull.Value },
             new SqlParameter("@ExtImagen", SqlDbType.VarChar, 500) { Value = (object)pro.ExtImagen ?? DBNull.Value },
             parametroMensaje,
             parametroResultado,
            };

            cn.Escribir("AgregarProducto", parametros);

            Mensaje = parametroMensaje.Value?.ToString();
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }
        public DataTable ListarProductos()
        {
            return cn.Leer("ListarProductos");
        }

        public int EliminarProducto(int id, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializamos por si no se asigna valor.

            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@Id_Producto",id),
            parametroMensaje,
            parametroResultado
            };


            cn.Escribir("ElimnarProducto", parametros);
            // Obtener los valores de los parámetros de salida
            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }


        public DataTable GetImgProd(int id)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@Id_Producto",id)
            };

            return cn.Leer("ObtenerImgProd", sp);
        }

        public int EditarPro(Productos pro, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializamos por si no se asigna valor.

            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] parametros = new SqlParameter[]
            {
             new SqlParameter("@Id_Producto",pro.Id_Producto),
             new SqlParameter("@Id_Linea",pro.Id_Linea),
             new SqlParameter("@Descripcion",pro.Descripcion),
             new SqlParameter("@Activo",pro.Activo),
             new SqlParameter("@Id_Categoria",pro.Id_Categoria),
             new SqlParameter("@Id_Catalogo",pro.Id_Catalogo),
             new SqlParameter("@Cod_Producto",pro.Cod_Producto),
             new SqlParameter("@Stock",pro.stock),
             new SqlParameter("@Precio",pro.Precio),
             new SqlParameter("@Electrico",pro.Electrico),
             new SqlParameter("@Nombre",pro.Nombre),
             new SqlParameter("@Imagen", SqlDbType.VarBinary) { Value = (object)pro.Imagen ?? DBNull.Value },
             new SqlParameter("@ExtImagen", SqlDbType.VarChar, 500) { Value = (object)pro.ExtImagen ?? DBNull.Value },
              parametroMensaje,
              parametroResultado
            };

            cn.Escribir("UpdateProducto", parametros);

            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
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
                    Imagen = row["Imagen"] != DBNull.Value ? (byte[])row["Imagen"] : null,

                });
            }

            return productos;
        }

    }
}
