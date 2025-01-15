using BusinessLogicalLayer;
using CostaAzulWeb.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection.Metadata;
using Document = iTextSharp.text.Document;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
 using QuestPDF.Elements.Text.Items;
using PageSize = iTextSharp.text.PageSize;
using iTextSharp.text.pdf.draw;
namespace CostaAzulWeb.Controllers
{
    public class CarritoController : Controller
    {
        private readonly BLL_Carrito car;

        public CarritoController()
        {
            car = new BLL_Carrito();

        }

        [HttpPost]
        public IActionResult AgregarProducto(int Id_Producto)
        {
            try
            {
                string ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipUsuario == "::1") // Detecta el loopback
                {
                    ipUsuario = "192.168.1.100"; // Reemplázalo con una IP de ejemplo
                }
                car.AgregarProductoCarrito(Id_Producto, ipUsuario);
                TempData["Message"] = "El producto fue agregado al carrito correctamente.";
                return RedirectToAction("Lista", "Productos");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al agregar un producto al carrito" + ex.Message;
                return View("Productos");
            }

        }


        [HttpGet]
        public IActionResult VerCarrito()
        {
            try
            {
                string ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipUsuario == "::1") // Detecta el loopback
                {
                    ipUsuario = "192.168.1.100"; // Reemplázalo con una IP de ejemplo
                }

                var carrito = car.VerCarrito(ipUsuario);

                if (carrito == null)
                {
                    // Si el carrito no existe, se crea un modelo vacío
                    var modelVacio = new CarritoViewModel
                    {
                        Id_Carrito = 0, // Indica que no hay un carrito asociado
                        Ip_Cliente = ipUsuario,
                        Productos = new List<Productos>(), // Lista vacía
                        Total = 0,
                        Subtotal = 0
                    };

                    TempData["Message"] = "Tu carrito está vacío."; // Mensaje informativo
                    return View("Carrito", modelVacio);
                }

                // Si el carrito existe, se pasa al modelo
                var model = new CarritoViewModel
                {
                    Id_Carrito = carrito.Id_Carrito,
                    Ip_Cliente = carrito.Ip_Cliente,
                    Productos = carrito.lista,
                    Total = carrito.Total,
                    Subtotal = carrito.Subtotal
                };

                return View("Carrito", model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al mostrar el carrito. Intenta nuevamente.";
                // Retorna un modelo vacío para evitar errores en la vista
                var modelError = new CarritoViewModel
                {
                    Id_Carrito = 0,
                    Ip_Cliente = "",
                    Productos = new List<Productos>(),
                    Total = 0,
                    Subtotal = 0
                };
                return View("Carrito", modelError);
            }
        }


        [HttpPost]
        public IActionResult EliminarProducto(int Id_Producto, int Id_Carrito)
        {
            try
            {
                car.EliminarProductoCarrito(Id_Producto, Id_Carrito);

                TempData["Message"] = "El producto fue eliminado correctamente.";

                return RedirectToAction("VerCarrito", "Carrito");



            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo eliminar el producto";
                return RedirectToAction("VerCarrito", "Carrito");
            }

        }



        [HttpPost]
        public IActionResult FinalizarCompra(CarritoViewModel cm)
        {
            string ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipUsuario == "::1") // Detecta el loopback
            {
                ipUsuario = "192.168.1.100"; // Reemplázalo con una IP de ejemplo
            }

            var carrito = car.VerCarrito(ipUsuario);

            var model = new CarritoViewModel
            {
                Ip_Cliente = ipUsuario,
                Id_Carrito = carrito.Id_Carrito,
                Subtotal = carrito.Subtotal,
                Total = carrito.Total,
                Nombre = cm.Nombre,
                Apellido = cm.Apellido,
            };



            return View("FinalizarCompra", model);




        }

        [HttpPost]
        public IActionResult Facturar(CarritoViewModel cm)
        {
            try
            {
                string ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (ipUsuario == "::1") // Detecta el loopback
                {
                    ipUsuario = "192.168.1.100"; // Reemplázalo con una IP de ejemplo
                }

                var carrito = car.VerCarrito(ipUsuario);

                var model = new CarritoModel
                {
                    Ip_Cliente = ipUsuario,
                    Id_Carrito = carrito.Id_Carrito,
                    Nombre = cm.Nombre,
                    Apellido = cm.Apellido,
                    Mail = cm.Mail,
                    Telefono = cm.Telefono,
                    FormaPago = cm.FormaPago,
                    FormaEntrega = cm.FormaEntrega,
                    Subtotal = carrito.Subtotal,
                    Total = carrito.Total,
                    Productos = carrito.lista,
                };

                var archivoFactura = GenerarFactura(model);

                car.Finalizar(model); // Finalizar el carrito

                if (archivoFactura == null || archivoFactura.Length == 0)
                {
                    return BadRequest("No se pudo generar el archivo PDF.");
                }

                var nombreArchivo = $"Factura_CostaAzul_{carrito.Id_Carrito}.pdf";

                // Retorna el archivo directamente
                return File(archivoFactura, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al facturar";
                return RedirectToAction("VerCarrito", "Carrito");
            }
        }

        [HttpGet]
        public IActionResult CantidadProductos()
        {
            try
            {
                string ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (ipUsuario == "::1") ipUsuario = "192.168.1.100";

                var carrito = car.VerCarrito(ipUsuario);

                // Calcular la cantidad total de productos
                int cantidadProductos = car.CantProductos(ipUsuario);

                return Json(cantidadProductos);
            }
            catch (Exception)
            {
                return Json(0); // Devuelve 0 en caso de error
            }
            
        }



        public byte[] GenerarFactura(CarritoModel carrito)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Crear el documento PDF
                var documento = new Document(PageSize.A4, 40, 40, 60, 60);
                PdfWriter.GetInstance(documento, memoryStream);

                documento.Open();

                // Fuentes
                var fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLUE);
                var fuenteSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
                var fuenteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

                // Encabezado
                var encabezado = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };
                encabezado.SetWidths(new float[] { 70, 30 }); // Proporciones de columnas

                // Logo y título
                var logo = iTextSharp.text.Image.GetInstance("wwwroot/img/logo.jpg"); // Ruta de tu logo
                logo.ScaleAbsolute(100, 50); // Escalar logo
                encabezado.AddCell(new PdfPCell(logo) { Border = 0, Rowspan = 2 });
                encabezado.AddCell(new PdfPCell(new Phrase($"Factura N°: {carrito.Id_Carrito}", fuenteTitulo))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });
                encabezado.AddCell(new PdfPCell(new Phrase($"Fecha: {DateTime.Now:dd/MM/yyyy}", fuenteSubtitulo))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });

                documento.Add(encabezado);
                documento.Add(new Paragraph("\n")); // Espaciado

                // Línea separadora
                var separador = new LineSeparator(1f, 100f, BaseColor.BLUE, Element.ALIGN_CENTER, -2);
                documento.Add(separador);

                // Datos del Cliente
                var datosCliente = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };
                datosCliente.AddCell(new PdfPCell(new Phrase("Factura de Compra", fuenteTitulo))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingBottom = 10
                });
                datosCliente.AddCell(new PdfPCell(new Phrase($"Cliente: {carrito.Nombre} {carrito.Apellido}", fuenteNormal)) { Border = 0 });
                datosCliente.AddCell(new PdfPCell(new Phrase($"Teléfono: {carrito.Telefono}", fuenteNormal)) { Border = 0 });
                datosCliente.AddCell(new PdfPCell(new Phrase($"Correo: {carrito.Mail}", fuenteNormal)) { Border = 0 });

                documento.Add(datosCliente);

                // Tabla de Productos
                var tablaProductos = new PdfPTable(5)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10f
                };
                tablaProductos.SetWidths(new float[] { 10, 50, 10, 15, 15 }); // Proporciones de columnas

                // Encabezados de la tabla
                var celdasEncabezado = new[] { "Cantidad", "Descripción", "Valor", "Costo Unitario", "Costo Total" };
                foreach (var texto in celdasEncabezado)
                {
                    tablaProductos.AddCell(new PdfPCell(new Phrase(texto, fuenteSubtitulo))
                    {
                        BackgroundColor = new BaseColor(220, 230, 241), // Azul claro
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }

                // Datos de productos
                foreach (var producto in carrito.Productos)
                {
                    tablaProductos.AddCell(new PdfPCell(new Phrase(producto.cant.ToString(), fuenteNormal)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tablaProductos.AddCell(new PdfPCell(new Phrase(producto.Descripcion, fuenteNormal)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${producto.Precio:F2}", fuenteNormal)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${producto.Precio:F2}", fuenteNormal)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${producto.Precio * producto.cant:F2}", fuenteNormal)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                documento.Add(tablaProductos);

                // Total
                var tablaTotales = new PdfPTable(2)
                {
                    WidthPercentage = 50,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    SpacingBefore = 10f
                };
                tablaTotales.AddCell(new PdfPCell(new Phrase("Subtotal:", fuenteSubtitulo)) { Border = 0 });
                tablaTotales.AddCell(new PdfPCell(new Phrase($"${carrito.Subtotal:F2}", fuenteNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                tablaTotales.AddCell(new PdfPCell(new Phrase("Total:", fuenteTitulo)) { Border = 0 });
                tablaTotales.AddCell(new PdfPCell(new Phrase($"${carrito.Total:F2}", fuenteTitulo)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                documento.Add(tablaTotales);

                // Pie de página
                var pieDePagina = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 20f
                };
                pieDePagina.AddCell(new PdfPCell(new Phrase("Gracias por su compra en Costa Azul", fuenteNormal))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 20f
                });

                documento.Add(pieDePagina);

                // Cerrar el documento
                documento.Close();

                return memoryStream.ToArray();
            }
        }






    }
}

