using CapaDatos.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.BLL
{
    public class BLL_Producto
    {
        Mp_Productos obj = new Mp_Productos();

        public DataTable GetAllProductos()
        {
            return obj.ListarProductos();
        }

        public DataTable BuscarProductos(string texto)
        {
            return obj.BuscarProductos(texto);
        }

        public int RegistrarProducto(Productos pr, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(pr.Nombre) || string.IsNullOrWhiteSpace(pr.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(pr.Descripcion) || string.IsNullOrWhiteSpace(pr.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(pr.Cod_Producto) || string.IsNullOrWhiteSpace(pr.Cod_Producto))
            {
                Mensaje = "El codigo del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Convert.ToInt32(pr.stock).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(pr.stock).ToString()))
            {
                Mensaje = "El stock del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Convert.ToInt32(pr.Precio).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(pr.Precio).ToString()))
            {
                Mensaje = "El Precio del producto no puede estar vacio";
            }

            if (pr.stock <= 0)
            {
                Mensaje = "El Producto no puede ser Negativo ni 0";
            }
            if(pr.Precio <= 0)
            {
                Mensaje = "El Precio del producto no puede ser 0 ni negativo";
            }

            if (pr.Imagen == null)
            {
                pr.Imagen = new byte[0];
            }
            if (string.IsNullOrEmpty(pr.ExtImagen))
            {
                pr.ExtImagen = "";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return obj.AgregarProducto(pr, out Mensaje);

            }
            else
            {
                return 0;
            }

        }
        public int UpdateProducto(Productos pr, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(pr.Nombre) || string.IsNullOrWhiteSpace(pr.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(pr.Descripcion) || string.IsNullOrWhiteSpace(pr.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(pr.Cod_Producto) || string.IsNullOrWhiteSpace(pr.Cod_Producto))
            {
                Mensaje = "El codigo del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Convert.ToInt32(pr.stock).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(pr.stock).ToString()))
            {
                Mensaje = "El stock del producto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Convert.ToInt32(pr.Precio).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(pr.Precio).ToString()))
            {
                Mensaje = "El Precio del producto no puede estar vacio";
            }

            if (pr.stock <= 0)
            {
                Mensaje = "El Producto no puede ser Negativo ni 0";
            }
            if (pr.Precio <= 0)
            {
                Mensaje = "El Precio del producto no puede ser 0 ni negativo";
            }

            if (pr.Imagen == null)
            {
                DataTable dt = obj.GetImgProd(pr.Id_Producto);

                if(dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    byte[] imagen = row["Imagen"] as byte[];
                    string extImagen = row["ExtImagen"].ToString();

                    if (imagen != null && imagen.Length > 0)
                    {
                        pr.Imagen = imagen;
                        pr.ExtImagen = extImagen;
                    }
                }
                else
                {
                    pr.Imagen = new byte[0];
                    pr.ExtImagen = "";
                }
       
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.EditarPro(pr, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public int EliminarProducto(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(Convert.ToInt32(id).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(id).ToString()))
            {
                Mensaje = "El id del Prodcuto no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.EliminarProducto(id, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
    }
}
