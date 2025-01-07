using BusinessLogicLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CostaAzulWeb.Models;

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

            return View();
        }

        [HttpPost]
        public IActionResult ValidUser(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Por favor, complete todos los campos requeridos.";
                return View("Login");
            }

            try
            {
                var user = _loginService.autenticar(model.Username, model.Password);

                if (user.Rows.Count > 0)
                {
                    var usuario = new Usuario
                    {
                        Nombre = model.Username,
                        Password = model.Password
                    };

                    // Iniciar sesión
                    HttpContext.Session.SetString("SesionIniciada", "true");
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));

                    TempData["Message"] = "¡Has iniciado sesión correctamente!";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error al intentar iniciar sesión: {ex.Message}");
            }

            return View("Login");
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


    }
}
