using BusinessLogicLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFormLayer.Models;
using System.Web.Mvc;
using System.Web.Services.Description;
using ServiceLayer;

namespace WebFormLayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        SessionManager sm = new SessionManager();

        // Constructor para inyectar LoginService
        public LoginController()
        {
            _loginService = new LoginService();  // Inicializa el servicio de login
        }

        // Acción GET: Muestra el formulario de login
        [HttpGet]
        public ActionResult Login()
        {
            return View();  // Retorna la vista de login vacía
        }


      



        // Acción POST: Procesa los datos del formulario de login
        [HttpPost]

        public ActionResult ValidUser(LoginViewModel model)
        {
            try
            {
                // Verificamos si ya hay una sesión activa
                if (SessionManager.GetInstance != null && SessionManager.GetInstance.SesionIniciada)
                {
                    // Si la sesión ya está activa, redirigimos al usuario a la página principal
                    TempData["Message"] = "¡Ya has iniciado sesión!";
                    
                }

                // Si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Usamos el LoginService para validar el usuario
                    var user = _loginService.autenticar(model.Username, model.Password);

                    // Verificamos si el usuario fue encontrado y tiene resultados válidos
                    if (user.Rows.Count > 0)
                    {
                        // Si el usuario es válido, guardamos la sesión usando el SessionManager
                        try
                        {
                            // Crear el objeto usuario
                            Usuario us = new Usuario
                            {
                                Nombre = model.Username,
                                Password = model.Password
                            };

                            // Realizamos el login
                            sm.Login(us.Nombre);  // Aquí gestionamos el login

                            // Guardamos el usuario en la sesión
                            Session["Usuario"] = us;
                            TempData["Message"] = "¡Has iniciado sesión correctamente!";

                            // Redirigimos a la página principal
                            return RedirectToAction("Index", "Home");
                        }
                        catch (Exception ex)
                        {
                            // Si ocurre un error al intentar iniciar sesión
                            TempData["Message"] = "Error al intentar iniciar sesión: " + ex.Message;
                        }
                    }
                    else
                    {
                        // Si las credenciales son incorrectas, mostramos un error
                        ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Ocurrió un error al intentar iniciar sesión.");
            }

            // Si no se ha podido autenticar o hubo un error, permanecemos en la vista de login
            return View(model);  // Mantenemos al usuario en la página de login
        }


        [HttpPost]
        public ActionResult Logout()
        {
            SessionManager.Logout();  // Elimina la cookie y cierra sesión

            TempData["Message"] = "¡Has cerrado sesión correctamente!";
            return RedirectToAction("Index", "Home");  // Redirige a la página principal
        }







    }
}