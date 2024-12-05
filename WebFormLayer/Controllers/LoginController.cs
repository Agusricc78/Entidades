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

        
        public LoginController()
        {
            _loginService = new LoginService();  
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();  
        }


        [HttpGet]
        public ActionResult AgregarUsuario()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registrar()
        {
            return RedirectToAction("AgregarUsuario", "Login");
        }
      

        [HttpPost]

        public ActionResult ValidUser(LoginViewModel model)
        {
            try
            {
                
                if (SessionManager.GetInstance != null && SessionManager.GetInstance.SesionIniciada)
                {
                    TempData["Message"] = "¡Ya has iniciado sesión!";
                    
                }

               
                if (ModelState.IsValid)
                {
                    
                    var user = _loginService.autenticar(model.Username, model.Password);

                    if (user.Rows.Count > 0)
                    {
                       
                        try
                        {
                           
                            Usuario us = new Usuario
                            {
                                Nombre = model.Username,
                                Password = model.Password
                            };

                            
                            sm.Login(us.Nombre);  

                           
                            Session["Usuario"] = us;
                            TempData["Message"] = "¡Has iniciado sesión correctamente!";

                            
                            return RedirectToAction("Index","Home");
                        }
                        catch (Exception ex)
                        {
                            TempData["Message"] = "Error al intentar iniciar sesión: " + ex.Message;
                        }
                    }
                    else
                    {
                       
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

           
            return View("Registrar","Shared");  
        }


        [HttpPost]
        public ActionResult Logout()
        {
            SessionManager.Logout();  // Elimina la cookie y cierra sesión

            TempData["Message"] = "¡Has cerrado sesión correctamente!";
            return RedirectToAction("Index", "Home");  // Redirige a la página principal
        }

        [HttpPost]

        public ActionResult AgregarUsuario(LoginViewModel model)
        {
            try
            {
                if(_loginService.ValidarExistencia(model.Username,model.Correo) == true)
                {
                    TempData["Message"] = "Usuario ya existe";

                }
                else
                {
                    var usuario = new Usuario
                    {
                        Nombre = model.Username,
                        Password = model.Password,
                        Correo = model.Correo,
                        Telefono = model.Telefono,
                         Activo = true,
                    };
                    _loginService.AgregarUser(usuario.Nombre,usuario.Telefono,usuario.Correo,usuario.Password);

                    TempData["Message"] = "Usuario agregado correctamente";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex) 
            {
                TempData["Message"] = ex.Message;
            }
            TempData["Message"] = "Usuario ya existe";
            return View(model);

         }





    }
}