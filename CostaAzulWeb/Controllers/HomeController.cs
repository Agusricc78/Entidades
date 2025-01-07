using CostaAzulWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Entities;
using Newtonsoft.Json;

namespace CostaAzulWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Verificar si existe la sesión y si está activa
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(usuarioJson))
            {
                // Deserializar el objeto Usuario desde la sesión
                var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                ViewBag.IsLoggedIn = true;
                ViewBag.UserName = usuario?.Nombre ?? "Usuario";
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
            // Verificar si existe la sesión activa
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuarioJson))
            {
                // Si no hay sesión activa, mostrar la vista de login
                return View("Login");
            }

            // Si ya hay una sesión activa, redirigir al índice
            return RedirectToAction("Index", "Home");
        }





        public ActionResult Contact()

        {


            return View();
        }
    }
}
