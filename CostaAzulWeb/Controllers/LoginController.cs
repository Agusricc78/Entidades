using BusinessLogicLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CostaAzulWeb.Models;
using System.Reflection;

namespace CostaAzulWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly BLL_Login _loginService;

        public LoginController(BLL_Login loginService)
        {
            _loginService = loginService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("SesionIniciada") == "true")
            {
              
                TempData["Message"] = "¡Ya has iniciado sesión!";
                return RedirectToAction("Index", "Home");
            }

            return PartialView("Login");
        }



        [HttpPost]
        public JsonResult ValidUser(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Por favor, complete todos los campos requeridos." });
            }

            try
            {
                var user = _loginService.autenticar(model.Username, model.Password);
                var log = _loginService.ObtenerUserXNombre(model.Username);

                log = new Usuario
                {
                    Nombre = model.Username,
                    Password = model.Password,
                    Id_Usuario = log.Id_Usuario

                };


                if (user.Rows.Count > 0)
                {
                    // Si las credenciales son correctas, iniciamos la sesión
                    var usuario = new Usuario
                    {
                        Nombre = model.Username,
                        Password = model.Password
                    };

                    // Guardamos la sesión
                    HttpContext.Session.SetString("SesionIniciada", "true");
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetInt32("UserId", log.Id_Usuario);// Guardamos que el usuario está logueado
                    ViewBag.IsLoggedIn = true;
                    return Json(new { success = true });
                }
                else
                {
                    // Si el usuario no existe o la contraseña es incorrecta, mostramos un error
                    return Json(new { success = false, message = "Usuario o contraseña incorrectos." });
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return Json(new { success = false, message = "Ocurrió un error al intentar iniciar sesión. Intente de nuevo." });
            }
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "¡Has cerrado sesión correctamente!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarUsuario(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Por favor, complete todos los campos requeridos.";
                return View(model);
            }

            try
            {
                if (_loginService.ValidarExistencia(model.Username, model.Correo))
                {
                    TempData["Message"] = "El usuario ya existe.";
                    return View(model);
                }

                var usuario = new Usuario
                {
                    Nombre = model.Username,
                    Password = model.Password,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Activo = true,
                };

                _loginService.AgregarUser(usuario.Nombre, usuario.Telefono, usuario.Correo, usuario.Password);

                TempData["Message"] = "Usuario agregado correctamente.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al agregar usuario: {ex.Message}";
                return View(model);
            }
        }


        [HttpPost]
        public IActionResult Registrar()
        {
            return RedirectToAction("AgregarUsuario");
        }


        [HttpGet]
        public JsonResult Estado()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
            var userId = HttpContext.Session.GetInt32("UserId");
            return Json(new { isLoggedIn, userId });
        }


    }
}
