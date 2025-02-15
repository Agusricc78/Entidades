using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using CapaNegocio;
using System.Data;
using CapaNegocio.BLL;
using System.Web.Helpers;
using System.Web.Services.Description;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using MercadoPago.Resource;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        private readonly HttpClient _httpClient;

        public TiendaController()
        {
            _httpClient = new HttpClient();
        }
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductosElectricos()
        {
            return View();
        }

        public ActionResult DetalleProducto(int Id_Producto = 0)
        {
            Productos pr = new Productos();
            var productosDatatable = new BLL_Producto().GetAllProductos();
            List<Productos> productos = ConvertirDataTableALista(productosDatatable);

            pr = productos.Where(P => P.Id_Producto == Id_Producto).FirstOrDefault();

            if (pr != null)
            {
                pr.Imagen = pr.Imagen;
                pr.ExtImagen = pr.ExtImagen;
            }

            return View(pr);
        }

        [HttpGet]

        public JsonResult GetCategorias()
        {
            DataTable dt = new BLL_Categoria().GetAllCategorias();

            var categorias = dt.AsEnumerable()
                    .Select(row => new
                    {
                        Id_Categoria = row["Id_Categoria"],
                        Nombre = row["Nombre"]
                    }).ToList();


            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public JsonResult GetLineasCategoria(int Id_Categoria)
        {
            DataTable dt = new BLL_Linea().GetLineaCategoria(Id_Categoria);

            var lineas = dt.AsEnumerable()
                .Select(row => new
                {
                    Id_Linea = row["Id_Linea"],
                    Nombre = row["Nombre"]
                });

            return Json(new { data = lineas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetProvincias()
        {
            DataTable dt = new BLL_Ubicacion().GetProvincias();

            var provincias = dt.AsEnumerable()
                .Select(row => new
                {
                    Id_Provincia = row["Id_Provincia"],
                    Nombre = row["Nombre"]
                });

            return Json(new { data = provincias }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetPartidosXProvincia(int idprovincia)
        {
            DataTable dt = new BLL_Ubicacion().GetPartidosXProvincia(idprovincia);

            var partido = dt.AsEnumerable()
                .Select(row => new
                {
                    Id_Partido = row["Id_Partido"],
                    Id_Provincia = row["Id_Provincia"],
                    Nombre = row["Nombre"]
                });

            return Json(new { data = partido }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLocalidadXPartido(int idpartido)
        {
            DataTable dt = new BLL_Ubicacion().GetLocalidadXPartido(idpartido);

            var localidad = dt.AsEnumerable()
                .Select(row => new
                {
                    Id_Localidad = row["Id_Localidad"],
                    Id_Partido = row["Id_Partido"],
                    Nombre = row["Nombre"]
                });

            return Json(new { data = localidad }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult getProductos(int Id_Categoria, int Id_Linea, int Id_Catalogo, int Elecrico)
        {
            var productosDatatable = new BLL_Producto().GetAllProductos();
            List<Productos> productos = ConvertirDataTableALista(productosDatatable);

            var productosFiltrados = productos
                .Where(P => P.Id_Categoria == (Id_Categoria == 0 ? P.Id_Categoria : Id_Categoria) &&
                            P.Id_Linea == (Id_Linea == 0 ? P.Id_Linea : Id_Linea) &&
                            P.Id_Catalogo == (Id_Catalogo == 0 ? P.Id_Catalogo : Id_Catalogo) &&
                            P.Electrico == (Elecrico == 0 ? P.Electrico : Elecrico) &&
                            P.Activo)
                .Select(P => new
                {
                    P.Id_Producto,
                    P.Cod_Producto,
                    P.Nombre,
                    P.Descripcion,
                    P.Id_Categoria,
                    P.Id_Linea,
                    P.Id_Catalogo,
                    P.stock,
                    P.Precio,
                    Imagen = P.Imagen != null ? Convert.ToBase64String(P.Imagen) : null,  // Convertir aquí
                    P.ExtImagen,
                    P.Activo,
                    P.Electrico
                })
                .ToList();

            var jsonresult = Json(new { data = productosFiltrados }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        private List<Productos> ConvertirDataTableALista(DataTable dt)
        {
            List<Productos> producto = new List<Productos>();
            foreach (DataRow row in dt.Rows)
            {
                byte[] imagenData = null;
                string ExtImangen = null;

                // Si la columna Imagen tiene datos binarios (byte[]), asignarlos
                if (row["Imagen"] != DBNull.Value && row["Imagen"] is byte[])
                {
                    imagenData = (byte[])row["Imagen"];
                    ExtImangen = row["ExtImagen"].ToString();

                }
                // Si la columna Imagen tiene un nombre de archivo y la columna ExtImagen tiene la extensión
                else if (row["Imagen"] != DBNull.Value && row["ExtImagen"] != DBNull.Value)
                {
                    string imagenArchivo = row["Imagen"].ToString();  // Nombre del archivo
                    string extImagen = row["ExtImagen"].ToString();  // Extensión de la imagen

                    if (!string.IsNullOrEmpty(imagenArchivo) && !string.IsNullOrEmpty(extImagen))
                    {
                        // Construir la URL completa para la imagen
                        ExtImangen = extImagen;
                    }
                }

                producto.Add(new Productos
                {
                    Id_Producto = Convert.ToInt32(row["Id_Producto"].ToString()),
                    Cod_Producto = row["Cod_Producto"].ToString(),
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Id_Categoria = Convert.ToInt32(row["Id_Categoria"].ToString()),
                    Id_Linea = Convert.ToInt32(row["Id_Linea"].ToString()),
                    Id_Catalogo = Convert.ToInt32(row["Id_Catalogo"].ToString()),
                    stock = Convert.ToInt32(row["Stock"].ToString()),
                    Precio = Convert.ToDecimal(row["Precio"].ToString()),
                    Imagen = imagenData,
                    ExtImagen = ExtImangen,
                    Activo = Convert.ToBoolean(row["Activo"].ToString()),
                    Electrico = Convert.ToInt32(row["Electrico"].ToString())
                });
            }
            return producto;
        }


        [HttpPost]

        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = 0;

            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // Si no hay "X-Forwarded-For", usamos la IP de la solicitud directa
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }


            if (Session["Cliente"] == null)
            {
                idcliente = 0;
            }
            else
            {
                idcliente = ((Usuario)Session["Cliente"]).Id_Usuario;
            }

            int cantidad = 0;
            int existe = new BLL_Carrito().ExisteCarrito(ipAddress, idproducto, out cantidad);

            int respuesta = 0;

            string Mensaje = string.Empty;

            if (existe > 0)
            {
                Mensaje = "El producto ya exsite en el carrito";
            }
            else
            {
                respuesta = new BLL_Carrito().OperacionCarrito(idcliente, idproducto, ipAddress, true, out Mensaje);
            }


            return Json(new { respuesta = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExisteProductoCarrito(int idproducto)
        {

            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            int respuesta = 0;

            // Si no hay "X-Forwarded-For", usamos la IP de la solicitud directa
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            int cantidad = 0;
            respuesta = new BLL_Carrito().ExisteCarrito(ipAddress, idproducto, out cantidad);

            return Json(new { respuesta = respuesta, cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public JsonResult CantidadEnCarrito()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // Si no hay "X-Forwarded-For", usamos la IP de la solicitud directa
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            object cantidad = new BLL_Carrito().CantidadEnCarrito(ipAddress);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductosCarrito()
        {
            try
            {
                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = Request.UserHostAddress;
                }

                if (ipAddress == "::1")
                {
                    ipAddress = "127.0.0.1";
                }

                var carritoDatatable = new BLL_Carrito().GetCarrito(ipAddress);

                if (carritoDatatable == null || carritoDatatable.Rows.Count == 0)
                {
                    return Json(new { error = "No se encontraron productos en el carrito." }, JsonRequestBehavior.AllowGet);
                }

                List<Carrito> carrito = ConvertirDataTableAListaCarrito(carritoDatatable).ToList();

                // Convertir imagen a Base64 para serializar en JSON
                var carritoJson = carrito.Select(c => new
                {
                    c.producto.Id_Producto,
                    c.producto.Nombre,
                    c.producto.Cod_Producto,
                    c.producto.Precio,
                    Imagen = c.producto.Imagen != null ? Convert.ToBase64String(c.producto.Imagen) : null,
                    c.producto.ExtImagen,
                    c.Cantidad
                }).ToList();

                return Json(new { data = carritoJson }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]

        public JsonResult OperacioCarrito(int idproducto, bool sumar)
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            int idcliente = 0;
            string Mensaje = string.Empty;
            // Si no hay "X-Forwarded-For", usamos la IP de la solicitud directa
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            if (Session["Cliente"] == null)
            {
                idcliente = 0;
            }
            else
            {
                idcliente = ((Usuario)Session["Cliente"]).Id_Usuario;
            }

            int respuesta = new BLL_Carrito().OperacionCarrito(idcliente, idproducto, ipAddress, sumar, out Mensaje);

            return Json(new { respuesta = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {

            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            string Mensaje = string.Empty;
            // Si no hay "X-Forwarded-For", usamos la IP de la solicitud directa
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            int respuesta = new BLL_Carrito().EliminarCarrito(ipAddress, idproducto);

            return Json(new { respuesta = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        private List<Carrito> ConvertirDataTableAListaCarrito(DataTable dt)
        {
            List<Carrito> carrito = new List<Carrito>();
            foreach (DataRow row in dt.Rows)
            {
                byte[] imagenData = null;
                string extImagen = null;

                // Manejo seguro de imágenes
                if (row["Imagen"] != DBNull.Value && row["Imagen"] is byte[])
                {
                    imagenData = (byte[])row["Imagen"];
                }
                extImagen = row["ExtImagen"] != DBNull.Value ? row["ExtImagen"].ToString() : "";

                // Agregar al carrito
                carrito.Add(new Carrito
                {
                    producto = new Productos()
                    {
                        Id_Producto = row["Id_Producto"] != DBNull.Value ? Convert.ToInt32(row["Id_Producto"]) : 0,
                        Nombre = row["Nombre"] != DBNull.Value ? row["Nombre"].ToString().Replace("\n", " ") : "",
                        Cod_Producto = row["Cod_Producto"] != DBNull.Value ? row["Cod_Producto"].ToString() : "",
                        Precio = row["Precio"] != DBNull.Value ? Convert.ToDecimal(row["Precio"]) : 0m,
                        Imagen = imagenData,
                        ExtImagen = extImagen
                    },
                    Cantidad = row["Cantidad"] != DBNull.Value ? Convert.ToInt32(row["Cantidad"]) : 1
                });
            }
            return carrito;
        }


        private List<DetalleVenta> ConvertirDataTableAListaDetalleVenta(DataTable dt)
        {
            List<DetalleVenta> dtventas = new List<DetalleVenta>();

            foreach (DataRow row in dt.Rows)
            {
                byte[] imagenData = null;
                string extImagen = null;

                // Manejo seguro de imágenes
                if (row["Imagen"] != DBNull.Value && row["Imagen"] is byte[])
                {
                    imagenData = (byte[])row["Imagen"];
                }
                extImagen = row["ExtImagen"] != DBNull.Value ? row["ExtImagen"].ToString() : "";

                // Agregar al carrito
                dtventas.Add(new DetalleVenta
                {
                    Productos = new Productos()
                    {
                        Nombre = row["Nombre"] != DBNull.Value ? row["Nombre"].ToString().Replace("\n", " ") : "",
                        Precio = row["Precio"] != DBNull.Value ? Convert.ToDecimal(row["Precio"]) : 0m,
                        Imagen = imagenData,
                        ExtImagen = extImagen
                    },
                    
                    Cantidad = row["Cantidad"] != DBNull.Value ? Convert.ToInt32(row["Cantidad"]) : 1,
                    Total = row["Precio"] != DBNull.Value ? Convert.ToDecimal(row["Precio"]) : 0m,
                    Ventas = new Ventas()
                    {
                        Id_Transaccion = row["Id_Transaccion"] != DBNull.Value ? row["Id_Transaccion"].ToString().Replace("\n", " ") : "",
                        NroPedido = row["NroPedido"] != DBNull.Value ? row["NroPedido"].ToString().Replace("\n", " ") : "",
                    }
                });
            }
            return dtventas;
        }

        public ActionResult Carrito() {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> listaCarrito, Ventas vt)
        {
            decimal Total = 0;
            string Mensaje = string.Empty;

            // Verificación de forma de pago
            if (vt.FormaPago == "1")
            {
                vt.FormaPago = "Efectivo";
            }
            else
            {
                vt.FormaPago = "Mercado Pago";
            }

            // Verificación de forma de retiro
            if (vt.FormaRetiro == "1")
            {
                vt.FormaRetiro = "Local";
            }
            else
            {
                vt.FormaRetiro = "Envio";
            }

            // Verifica la IP del cliente
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                vt.Ip_Cliente = Request.UserHostAddress;
            }
            if (vt.Ip_Cliente == "::1")
            {
                vt.Ip_Cliente = "127.0.0.1";
            }

            // Verificar si el usuario está autenticado
            if (Session["Cliente"] == null)
            {
                return Json(new { Status = false, mensaje = "Para poder procesar el pago debe iniciar sesión, por favor." });
            }

            // Obtener el ID del cliente
            vt.Id_Cliente = ((Usuario)Session["Cliente"]).Id_Usuario;

            // Crear la tabla de detalles de venta
            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-AR");
            dt.Columns.Add("Id_Producto", typeof(int));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("NombreProducto", typeof(string));

            foreach (Carrito carrito in listaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(carrito.Cantidad.ToString()) * carrito.producto.Precio;
                Total += subtotal;

                dt.Rows.Add(new object[] { carrito.producto.Id_Producto, carrito.Cantidad, subtotal, carrito.producto.Nombre });
            }

            // Guardar información en TempData
            vt.MontoTotal = Total;
            vt.Id_Estado = 2;
            vt.TotalProductos = dt.Rows.Count;

            object IdCorrelativo = new BLL_Venta().ObtenerCorrelativo();
            string NroPedido = string.Format("{0:00000}", IdCorrelativo);

            vt.NroPedido = NroPedido;

            Session["DetalleVenta"] = dt;
            Session["Venta"] = vt;


            // Verificar si la forma de pago es Mercado Pago
            if (vt.FormaPago == "Mercado Pago")
            {
                // Llamar a CrearPreferencias para generar la URL de pago
                string urlPago = await CrearPreferencias(); // Obtenemos la URL directamente desde el método

                // Verificar si la URL fue obtenida correctamente
                if (!string.IsNullOrEmpty(urlPago))
                {
                    // Redirigir al usuario a la URL de Mercado Pago
                    return Json(new { Status = true, Link = urlPago });
                }
                else
                {
                    return Json(new { Status = false, mensaje = "Hubo un error al generar la preferencia de pago. Intenta nuevamente." });
                }
            }

            Random random = new Random();
            int paymentId = random.Next(10000000, 99999999);  // Genera un número aleatorio de 8 dígitos
            return Json(new { Status = true, Link = $"/Tienda/PagoEfectuado?payment_id={paymentId}&status=true" });

        }

        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((Usuario)Session["Cliente"]).Id_Usuario;

            var ventasdatatable = new BLL_Venta().GetComprasCliente(idcliente);
            List<DetalleVenta> ventas = ConvertirDataTableAListaDetalleVenta(ventasdatatable);

            return View(ventas);
        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string paymentId = Request.QueryString["payment_id"]; 
            string statusString = Request.QueryString["status"]; 

            bool status = false;

            if (statusString == "approved" || statusString == "true")
            {
                status = true;
            }
            else if (statusString == "rejected" || statusString == "false")
            {
                status = false;
            }
            else
            {
                // Si es un valor no esperado
                ViewData["MensajeError"] = "El estado del pago no es válido.";
            }

            string Mensaje = string.Empty;

  
            ViewData["Status"] = status;

            if (status)
            {
                Ventas vt = (Ventas)Session["Venta"];
                DataTable detalle_venta = (DataTable)Session["DetalleVenta"];

         
                vt.Id_Transaccion = paymentId;

                string correo =  ((Usuario)Session["Cliente"]).Correo;

                int respuesta = new BLL_Venta().Registrar(vt, detalle_venta,correo, out Mensaje);

                if (respuesta > 0)
                {
                    ViewData["Id_Transaccion"] = vt.Id_Transaccion;
                }
                else
                {
                    ViewData["MensajeError"] = "Hubo un problema al registrar la venta. Intenta nuevamente.";
                }
            }
            // Si la transacción está fallida, mostramos un mensaje en la vista
            if (string.IsNullOrEmpty(ViewData["Id_Transaccion"].ToString()) && string.IsNullOrEmpty(ViewData["MensajeError"]?.ToString()))
            {
                ViewData["MensajeError"] = "Hubo un error en el proceso de pago. Por favor, contacta con el soporte.";
            }

            return View();
        }

        public async Task<string> CrearPreferencias()
        {
            string accessToken = "TEST-1611848299444913-021119-560741862cb0358a61684448b012a110-1048802911"; 

            var detalle_venta = (DataTable)Session["DetalleVenta"];
            var items = new List<object>();

            foreach (DataRow row in detalle_venta.Rows)
            {
                var item = new
                {
                    id = row["Id_Producto"].ToString(),
                    title = "Producto",
                    quantity = Convert.ToInt32(row["Cantidad"]),
                    currency_id = "ARS",
                    unit_price = Convert.ToDecimal(row["Total"])
                };

                items.Add(item);
            }

           
            var preferencia = new
            {
                items = items,
                back_urls = new
                {
                    success = $"https://localhost:44384/Tienda/PagoEfectuado?", 
                    failure = $"https://localhost:44384/Tienda/PagoEfectuado?",
                    pending = $"https://localhost:44384/Tienda/PagoEfectuado?"
                },
                auto_return = "approved" 
            };

            var jsonContent = JsonConvert.SerializeObject(preferencia);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = await _httpClient.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic responseJson = JsonConvert.DeserializeObject(responseBody);

            string paymentUrl = responseJson.init_point;

            return paymentUrl;
        }
    }
}
