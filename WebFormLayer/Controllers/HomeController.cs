using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFormLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sesion = SessionManager.GetInstance;
            if (sesion != null && sesion.SesionIniciada)
            {
              
                ViewBag.IsLoggedIn = true;
                ViewBag.UserName = sesion.Usuario.Nombre;
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

        public ActionResult Login()
        {
            var session = SessionManager.GetInstance;

            if (session == null || !session.SesionIniciada)
            {
               
                return View();  
            }

            
            return RedirectToAction("Index", "Home");
        }



        public ActionResult Contact()
        {
            

            return View();
        }
    }
}