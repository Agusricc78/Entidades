using BusinessLogicalLayer;
using CostaAzulWeb.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

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
                return RedirectToAction("Lista","Productos");
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
        public IActionResult EliminarProducto(int Id_Producto,int Id_Carrito)
        {
            try
            {
                car.EliminarProductoCarrito(Id_Producto, Id_Carrito);

                TempData["Message"] = "El producto fue eliminado correctamente.";

                return RedirectToAction("VerCarrito","Carrito");



            }
            catch(Exception ex)
            {
                TempData["Message"] = "No se pudo eliminar el producto";
                return RedirectToAction("VerCarrito", "Carrito");
            }

        }




    }
}
