using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFormLayer.Controllers
{
    public class PagoController : Controller
    {
        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }

        public PagoController()
        {
            // Configura las credenciales de MercadoPago
            SDK.Initialize("TU_CLIENT_ID", "TU_CLIENT_SECRET");
        }




    }
}