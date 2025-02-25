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
using PagedList;
using PagedList.Mvc;
using CapaPresentacionTienda.Models;
using System.Web.UI;

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
        public JsonResult getProductos(int Id_Categoria, int Id_Linea, int Id_Catalogo, string Elecrico, string nombreProducto, int page = 1, int pageSize = 6)
        {
            var productosDatatable = new BLL_Producto().GetAllProductos();
            List<Productos> productos = ConvertirDataTableALista(productosDatatable);

            var productosFiltrados = productos
                .Where(P => P.Id_Categoria == (Id_Categoria == 0 ? P.Id_Categoria : Id_Categoria) &&
                            P.Id_Linea == (Id_Linea == 0 ? P.Id_Linea : Id_Linea) &&
                            P.Id_Catalogo == (Id_Catalogo == 0 ? P.Id_Catalogo : Id_Catalogo) &&
                            (string.IsNullOrEmpty(Elecrico) || P.Electrico.ToLower().Contains(Elecrico.ToLower())) &&
                            (string.IsNullOrEmpty(nombreProducto) || P.Nombre.ToLower().Contains(nombreProducto.ToLower())) &&
                            P.Activo)
                .ToList();


            var totalProducts = productosFiltrados.Count();

            var productosPaginados = productosFiltrados
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                    Imagen = P.Imagen != null ? Convert.ToBase64String(P.Imagen) : null,  // Convertir imagen a base64
                    P.ExtImagen,
                    P.Activo,
                    P.Electrico
                })
                .ToList();


            var jsonresult = Json(new { data = productosPaginados, totalProducts = totalProducts }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        [HttpPost]

        public JsonResult BuscarProductos(string texto)
        {
            var Productos = new BLL_Producto().BuscarProductos(texto);

            List<Productos> productos = ConvertirDataTableDropdown(Productos);

            texto = texto.Trim().ToLower();

            var productosFiltrado = productos.Where(p => p.Nombre.ToLower().Contains(texto))
                .Select(p => new
                {
                    p.Id_Producto,
                    p.Nombre,
                    p.Descripcion,
                    Imagen = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null,  // Convertir aquí
                    p.ExtImagen,
                    p.Precio
                }).ToList();

            return Json(new {data = productosFiltrado, JsonRequestBehavior.AllowGet});
        }

        public void SetTokenInCookie(string token)
        {
            HttpCookie tokenCookie = new HttpCookie("UserToken", token);
            tokenCookie.Expires = DateTime.Now.AddDays(30);  
            Response.Cookies.Add(tokenCookie);
        }

        public string GetTokenFromCookie()
        {
            HttpCookie tokenCookie = Request.Cookies["UserToken"];
            if (tokenCookie != null)
            {
                return tokenCookie.Value;
            }

            return null; 
        }

        public string GenerarTokenUnico()
        {
            // Genera un GUID y toma los primeros 8 caracteres del hash
            var guid = Guid.NewGuid().ToString("N"); // Esto genera un GUID sin guiones
            return guid.Substring(0, 8); // Aquí tomas solo los primeros 8 caracteres
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
                    Electrico = row["Electrico"].ToString()
                });
            }
            return producto;
        }


        private List<Productos> ConvertirDataTableDropdown(DataTable dt)
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
                    Nombre = row["Nombre"].ToString(),
                    Descripcion = row["Descripcion"].ToString(),
                    Precio = Convert.ToDecimal(row["Precio"].ToString()),
                    Imagen = imagenData,
                    ExtImagen = ExtImangen
                });
            }
            return producto;
        }


        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = 0;

            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
                // Usar el token para lo que necesites
            }
            else
            {
                // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
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

            int existe = new BLL_Carrito().ExisteCarrito(token, idproducto, out cantidad);

            int respuesta = 0;

            string Mensaje = string.Empty;

            if (existe > 0)
            {
                Mensaje = "El producto ya exsite en el carrito";
            }
            else
            {
                respuesta = new BLL_Carrito().OperacionCarrito(idcliente, idproducto, token, true, out Mensaje);
            }


            return Json(new { respuesta = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExisteProductoCarrito(int idproducto)
        {

            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
                // Usar el token para lo que necesites
            }
            else
            {
                // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
            }

            int respuesta = 0;

            int cantidad = 0;
            respuesta = new BLL_Carrito().ExisteCarrito(token, idproducto, out cantidad);

            return Json(new { respuesta = respuesta, cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public JsonResult CantidadEnCarrito()
        {
            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
                // Usar el token para lo que necesites
            }
            else
            {
                // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
            }

            object cantidad = new BLL_Carrito().CantidadEnCarrito(token);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductosCarrito()
        {
            try
            {
                string token = GetTokenFromCookie();

                // Si el token es válido, lo puedes usar
                if (!string.IsNullOrEmpty(token))
                {
                    // Usar el token para lo que necesites
                }
                else
                {
                    // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                    token = GenerarTokenUnico();
                    SetTokenInCookie(token);
                }

                var carritoDatatable = new BLL_Carrito().GetCarrito(token);

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
          
            int idcliente = 0;
            string Mensaje = string.Empty;

            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
                // Usar el token para lo que necesites
            }
            else
            {
                // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
            }

            if (Session["Cliente"] == null)
            {
                idcliente = 0;
            }
            else
            {
                idcliente = ((Usuario)Session["Cliente"]).Id_Usuario;
            }

            int respuesta = new BLL_Carrito().OperacionCarrito(idcliente, idproducto, token, sumar, out Mensaje);

            return Json(new { respuesta = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {

           

            string Mensaje = string.Empty;
            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
                // Usar el token para lo que necesites
            }
            else
            {
                // Si no existe el token, podrías generar uno nuevo y almacenarlo en la cookie
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
            }

            int respuesta = new BLL_Carrito().EliminarCarrito(token, idproducto);

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

            // Validar forma de pago
            vt.FormaPago = (vt.FormaPago == "1") ? "Efectivo" : "Mercado Pago";
            vt.FormaRetiro = (vt.FormaRetiro == "1") ? "Local" : "Envio";

            // Obtener IP del cliente
            string token = GetTokenFromCookie();

            // Si el token es válido, lo puedes usar
            if (!string.IsNullOrEmpty(token))
            {
               vt.Ip_Cliente = token;
            }
            else
            {
                token = GenerarTokenUnico();
                SetTokenInCookie(token);
            }

            // Validar sesión del usuario
            if (Session["Cliente"] == null)
            {
                return Json(new { Status = false, mensaje = "La sesión ha expirado. Vuelve a iniciar sesión." });
            }

            vt.Id_Cliente = (Session["Cliente"] == null || Session["Cliente"] == DBNull.Value)
                            ? 1
                            : ((Usuario)Session["Cliente"]).Id_Usuario;

            // Crear tabla de detalle de venta
            DataTable dt = new DataTable();
            dt.Columns.Add("Id_Producto", typeof(int));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("NombreProducto", typeof(string));

            // Llenar el DataTable con los productos del carrito
            foreach (Carrito carrito in listaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(carrito.Cantidad) * carrito.producto.Precio;
                Total += subtotal;

                dt.Rows.Add(new object[] { carrito.producto.Id_Producto, carrito.Cantidad, subtotal, carrito.producto.Nombre });
            }

            // Validar si el detalle de venta está vacío antes de continuar
            if (dt.Rows.Count == 0)
            {
                return Json(new { Status = false, mensaje = "El carrito de compras está vacío." });
            }

            // Verificar que los Id_Producto existen en la base de datos antes de insertar

            Session["Venta"] = vt;
            Session["DetalleVenta"] = dt;
            // Guardar información en TempData
            vt.MontoTotal = Total;
            vt.Id_Estado = 1;
            vt.TotalProductos = dt.Rows.Count;

            // Generar número de pedido
            object IdCorrelativo = new BLL_Venta().ObtenerCorrelativo();
            vt.NroPedido = string.Format("{0:00000}", IdCorrelativo);


            
            // Si el pago es Mercado Pago, generar URL de pago
            if (vt.FormaPago == "Mercado Pago")
            {
                string urlPago = await CrearPreferencias(); // Método para generar el link de pago

                if (!string.IsNullOrEmpty(urlPago))
                {
                    return Json(new { Status = true, Link = urlPago });
                }
                else
                {
                    return Json(new { Status = false, mensaje = "Hubo un error al generar la preferencia de pago. Intenta nuevamente." });
                }
            }
            else
            {
                Random random = new Random();
                string paymentId = random.Next(10000000, 99999999).ToString();
             
                try
                {

                     return Json(new { Status = true, Link = $"/Tienda/PagoEfectuado?payment_id={paymentId}&status=true" });
        
                }
                catch (Exception ex)
                {
                    return Json(new { Status = false, mensaje = "Error en la inserción: " + ex.Message });
                }
            }
        }

    

    [ValidarSession]
    [Authorize]
    public ActionResult MisCompras(int? page)
    {
        int idcliente = ((Usuario)Session["Cliente"]).Id_Usuario;
        int pageSize = 3; // Número de pedidos por página
        int pageNumber = (page ?? 1); // Página actual

        var ventasdatatable = new BLL_Venta().GetComprasCliente(idcliente);
        List<DetalleVenta> ventas = ConvertirDataTableAListaDetalleVenta(ventasdatatable);

        var ventasViewModel = ventas
            .Select(v => new VentasViewModel
            {
                NroPedido = v.Ventas.NroPedido,
                Id_Transaccion = v.Ventas.Id_Transaccion,
                Imagen = v.Productos.Imagen != null ? Convert.ToBase64String(v.Productos.Imagen) : null,
                ExtImagen = v.Productos.ExtImagen,
                Nombre = v.Productos.Nombre,
                Precio = v.Productos.Precio,
                Cantidad = v.Cantidad,
                Total = v.Total
            })
            .GroupBy(v => v.NroPedido) // Agrupa por número de pedido
            .Select(g => new PaginatedVentas
            {
                NroPedido = g.Key,
                Id_Transaccion = g.First().Id_Transaccion,
                Productos = g.ToList()
            })
            .ToList();

        return View(ventasViewModel.ToPagedList(pageNumber, pageSize));
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
                ViewData["MensajeError"] = "El estado del pago no es válido.";
            }

            string Mensaje = string.Empty;

            ViewData["Status"] = status;

            if (status)
            {
                Ventas vt = ((Ventas)Session["Venta"]);
                DataTable dt = ((DataTable)Session["DetalleVenta"]);

                string correo = ((Usuario)Session["Cliente"]).Correo;

                vt.Id_Transaccion = paymentId;

                int respuesta = new BLL_Venta().Registrar(vt, dt,correo, out Mensaje);

                if(respuesta > 0)
                {
                    ViewData["Id_Transaccion"] = vt.Id_Transaccion;
                }
                else
                {
                    ViewData["MensajeError"] = "Hubo un problema al registrar la venta. Intenta nuevamente.";
                }

                if (string.IsNullOrEmpty(ViewData["Id_Transaccion"]?.ToString()) && string.IsNullOrEmpty(ViewData["MensajeError"]?.ToString()))
                {
                    ViewData["MensajeError"] = "Hubo un error en el proceso de pago. Por favor, contacta con el soporte.";
                }

                return View();
            }
            else
            {
                ViewData["MensajeError"] = "El pago no ha sido aprobado.";
                return View();
            }
        }


        public async Task<string> CrearPreferencias()
        {
            string accessToken = "APP_USR-1611848299444913-021119-ad9462fd68104d7c968d89690dd406b9-1048802911"; 

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
                    success = $"https://costaazul-d4bxcsfxe4fha5cv.brazilsouth-01.azurewebsites.net/Tienda/PagoEfectuado?", 
                    failure = $"https://costaazul-d4bxcsfxe4fha5cv.brazilsouth-01.azurewebsites.net/Tienda/PagoEfectuado?",
                    pending = $"https://costaazul-d4bxcsfxe4fha5cv.brazilsouth-01.azurewebsites.net/Tienda/PagoEfectuado?"
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
