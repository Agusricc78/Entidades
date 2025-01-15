using CostaAzulWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Entities;
using Newtonsoft.Json;
using BusinessLogicLayer;

namespace CostaAzulWeb.Controllers
{
    public class HomeController : Controller 
    {
        private readonly BLL_Login log;

        public HomeController()
        {
            log = new BLL_Login();
            
        }


        public IActionResult Index()
        {
            // Verificar si existe la sesión y si está activa
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(usuarioJson))
            {
                // Deserializar el objeto Usuario desde la sesión
                var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                var user = log.ObtenerUserXNombre(usuario?.Nombre);

                ViewBag.IsLoggedIn = true;
                ViewBag.UserName = user?.Nombre ?? "Usuario";
                ViewBag.UserId = user?.Id_Usuario;

            }
            else
            {
                ViewBag.IsLoggedIn = false;
            }

            return View();
        }


        public ActionResult Carrito()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            // Verificar si existe una sesión activa
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(usuarioJson))
            {
                // Si ya hay una sesión activa, configurar las variables necesarias
                ViewBag.IsLoggedIn = true;

                // Intentar deserializar el usuario para obtener más detalles
                try
                {
                    var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                    ViewBag.UserId = usuario.Id_Usuario;
                    ViewBag.UserName = usuario.Nombre;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error al deserializar la sesión: {ex.Message}";
                }

                // Redirigir al índice
                return RedirectToAction("Index", "Home");
            }

            // Si no hay sesión activa, configurar las variables y mostrar la vista de login
            ViewBag.IsLoggedIn = false;
            ViewBag.UserId = null;
            ViewBag.UserName = null;

            return View("Login");
        }





        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RenderNavbar()
        {
            // Verifica el estado de login desde la sesión
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId"); // Si tienes el UserId en sesión
            return PartialView("_NavbarPartial");
        }



    }
}
