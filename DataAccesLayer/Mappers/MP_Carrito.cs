using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccesLayer.Mappers
{
    public class MP_Carrito
    {
        private readonly Conexion cs = new Conexion();

        public int CrearCarrito(string ipCliente)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Ip_Cliente", ipCliente)
            };
            return cs.EscribirConRetorno("CrearCarrito", parametros);
        }

        public Carrito ObtenerCarritoPorIp(string ipCliente)
        {
            string storeProc = "ObtenerCarritoPorIp";

            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Ip_Cliente", ipCliente)
            };

            DataTable dt = cs.Leer(storeProc, parametros);

            if (dt.Rows.Count == 0)
            {
                return null; // No hay carrito para la IP dada
            }

            // Crear una instancia del carrito
            var carrito = new Carrito
            {
                Id_Carrito = Convert.ToInt32(dt.Rows[0]["Id_Carrito"]),
                Ip_Cliente = dt.Rows[0]["Ip_Cliente"].ToString(),
                Subtotal = Convert.ToDecimal(dt.Rows[0]["Subtotal"]),
                Total = Convert.ToDecimal(dt.Rows[0]["Total"]),

                lista = new List<Productos>()
            };

            // Llenar la lista de productos
            foreach (DataRow row in dt.Rows)
            {
                var producto = new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"]),

                    Descripcion = row["Descripcion"].ToString(),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    Cod_Producto = row["Cod_Producto"].ToString(),
                    Imagen = row["Imagen"].ToString(),
                    cant = Convert.ToInt32(row["Cantidad"].ToString())
                };

                carrito.lista.Add(producto);
            }

            return carrito;
        }


        public int AgregarProductos(int id_producto, string Ip_Cliente,int cantidad)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
               new SqlParameter("@Id_Producto", id_producto),
               new SqlParameter("@Ip_Cliente", Ip_Cliente),
               new SqlParameter("@Cantidad", cantidad),
            };
            return cs.Escribir("AgregarProductoConCarrito", parametros);

        }


        public int EliminarProducto(int id_carrito, int id_prod)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
               new SqlParameter("@Id_Carrito", id_carrito),
               new SqlParameter("@Id_Producto", id_prod),
            };
            return cs.Escribir("EliminarProductoDelCarrito", parametros);

        }


        public int FinalizarCompra(CarritoModel carrito)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Id_Carrito", carrito.Id_Carrito),
        new SqlParameter("@Nombre", carrito.Nombre ?? (object)DBNull.Value),
        new SqlParameter("@Apellido", carrito.Apellido ?? (object)DBNull.Value),
        new SqlParameter("@Mail", carrito.Mail ?? (object)DBNull.Value),
        new SqlParameter("@Telefono", carrito.Telefono ?? (object)DBNull.Value),
        new SqlParameter("@FormaPago", carrito.FormaPago ?? (object)DBNull.Value),
        new SqlParameter("@FormaEntrega", carrito.FormaEntrega ?? (object)DBNull.Value)
            };

            return cs.Escribir("FinalizarCompra", parametros);
        }


        public int CantProductos(string Ip_Cliente)
        {
            string storeProc = "ProductosCarrito";
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Ip_Cliente", Ip_Cliente),
            };

            object resultado = cs.ObetenerDatos("sp_ContarProductosCarrito", parametros);

            return resultado != null ? Convert.ToInt32(resultado) : 0; //


        }


        public DataTable PedidosFinalizados()
        {



            return cs.Leer("PedidosFinalizados");
        }


        public DataTable PedidosRetirados()
        {
            return cs.Leer("PedidosRetirados");
        }



        public List<CarritoModel> ObtenerCarritosPorEstado(string estado)
        {
            string storeProc = "sp_GetCarritosRetirados";

            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@Estado", estado)
            };

            DataTable dt = cs.Leer(storeProc, parametros);

            if (dt.Rows.Count == 0)
            {
                return null; // No hay carritos para el estado dado
            }

            // Lista de carritos
            var carritos = new List<CarritoModel>();
            var carritoDict = new Dictionary<int, CarritoModel>(); // Usar diccionario para evitar duplicados

            // Iterar sobre cada fila para llenar los carritos
            foreach (DataRow row in dt.Rows)
            {
                int idCarrito = Convert.ToInt32(row["Id_Carrito"]);

                // Verificar si el carrito ya existe en el diccionario
                if (!carritoDict.ContainsKey(idCarrito))
                {
                    // Crear una instancia del carrito
                    var carrito = new CarritoModel
                    {
                        Id_Carrito = idCarrito,
                        Ip_Cliente = row["Ip_Cliente"].ToString(),
                        Subtotal = Convert.ToDecimal(row["Subtotal"]),
                        Total = Convert.ToDecimal(row["Total"]),
                        Nombre = row["Nombre_Cliente"].ToString(),
                        Apellido = row["Apellido_Cliente"].ToString(),
                        Mail = row["Mail_Cliente"].ToString(),
                        Telefono = row["Telefono_Cliente"].ToString(),
                        FormaPago = row["Forma_Pago"].ToString(),
                        FormaEntrega = row["Forma_Entrega"].ToString(),
                        Estado = row["Estado"].ToString(),
                        Productos = new List<Productos>() // Inicializar lista de productos
                    };

                    carritoDict[idCarrito] = carrito; // Agregar al diccionario
                }

                // Agregar productos al carrito
                var producto = new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"]),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    Cod_Producto = row["Cod_Producto"].ToString(),
                    Imagen = row["Imagen"].ToString(),
                    cant = Convert.ToInt32(row["Cantidad"].ToString()),
                };

                carritoDict[idCarrito].Productos.Add(producto);
            }

            // Convertir el diccionario a lista
            carritos = carritoDict.Values.ToList();

            return carritos;
        }


        public List<CarritoModel> GetCarritos()
        {
            string storeProc = "GetCarritos";

            // Ejecutar el procedimiento almacenado sin parámetros
            DataTable dt = cs.Leer(storeProc);

            if (dt.Rows.Count == 0)
            {
                return new List<CarritoModel>(); // Retornar una lista vacía si no hay resultados
            }

            // Lista de carritos a retornar
            var carritos = new List<CarritoModel>();
            var carritoDict = new Dictionary<int, CarritoModel>(); // Diccionario para evitar duplicados

            // Recorrer cada fila de la tabla
            foreach (DataRow row in dt.Rows)
            {
                int idCarrito = Convert.ToInt32(row["Id_Carrito"]);

                // Verificar si el carrito ya existe en el diccionario
                if (!carritoDict.ContainsKey(idCarrito))
                {
                    // Crear y agregar un nuevo carrito
                    var carrito = new CarritoModel
                    {
                        Id_Carrito = idCarrito,
                        Ip_Cliente = row["Ip_Cliente"].ToString(),
                        Subtotal = Convert.ToDecimal(row["Subtotal"]),
                        Total = Convert.ToDecimal(row["Total"]),
                        Nombre = row["Nombre_Cliente"].ToString(),
                        Apellido = row["Apellido_Cliente"].ToString(),
                        Mail = row["Mail_Cliente"].ToString(),
                        Telefono = row["Telefono_Cliente"].ToString(),
                        FormaPago = row["Forma_Pago"].ToString(),
                        FormaEntrega = row["Forma_Entrega"].ToString(),
                        Estado  = row["Estado"].ToString(),
                        Productos = new List<Productos>() // Inicializar lista de productos
                    };

                    carritoDict[idCarrito] = carrito; // Agregar al diccionario
                }

                // Crear un producto y agregarlo a la lista de productos del carrito
                var producto = new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"]),
                    Precio = Convert.ToDecimal(row["Precio"]),
                    Imagen = row["Imagen"].ToString(),
                    cant = Convert.ToInt32(row["Cantidad"])
                };

                carritoDict[idCarrito].Productos.Add(producto);
            }

            // Convertir el diccionario a una lista y retornarla
            carritos = carritoDict.Values.ToList();
            return carritos;
        }


        public int ActualizarCant(int id_carrito, int id_prod, int cant)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
               new SqlParameter("@Id_Carrito", id_carrito),
               new SqlParameter("@Id_Producto", id_prod),
               new SqlParameter("@Cantidad", cant),
            };
            return cs.Escribir("sp_ActualizarCantidad", parametros);

        }




    }
}
