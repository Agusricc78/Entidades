using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using System.Data;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View(new Usuario());
        }

        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario objeto)
        {
            int resultado;
            string mensaje = string.Empty;

 

            ViewData["Nombre"] = string.IsNullOrEmpty(objeto.Nombre) ? "" : objeto.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(objeto.Apellido) ? "": objeto.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;


            if (string.IsNullOrEmpty(objeto.Password) || string.IsNullOrEmpty(objeto.confirmarClave))
            {
                ViewBag.Error = "Las contraseñas no pueden estar vacías";
                return View(objeto);
            }
            if (objeto.Password != objeto.confirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(objeto);
            }

            resultado = new BLL_Login().RegistrarCliente(objeto, out mensaje);

            if(resultado > 0 )
            {
                ViewBag.Error = null;
                return RedirectToAction("Index","Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View(objeto);
            }
        }

        [HttpPost]
        public ActionResult Index(string correo,string clave)
        {
            var usuariosDataTable = new BLL_Login().GetAllUsers();

            List<Usuario> usuarios = ConvertirDataTableALista(usuariosDataTable);

            string clavehash = BLL_Recursos.EncriptarContraseña(clave);

            Usuario usu = usuarios.FirstOrDefault(u => u.Correo == correo && u.Password == clavehash);

            if(usu == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (usu.Restablecer)
                {
                    TempData["Id_Usuario"] = usu.Id_Usuario;
                    return RedirectToAction("CambiarClave");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(usu.Correo, false);

                    Session["Cliente"] = usu;

                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            var usuariosDataTable = new BLL_Login().GetAllUsers();

            List<Usuario> usuarios = ConvertirDataTableALista(usuariosDataTable);


            Usuario usu = usuarios.FirstOrDefault(u => u.Correo == correo);

            if (usu == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;

            bool respuesta = new BLL_Login().RestablecerClave(usu.Id_Usuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(int idUsuario, string claveactual, string nuevaclave, string confirmarclave)
        {

            var usuariosDataTable = new BLL_Login().GetAllUsers();

            List<Usuario> usuarios = ConvertirDataTableALista(usuariosDataTable);


            Usuario usu = usuarios.FirstOrDefault(u => u.Id_Usuario == idUsuario);
            string claveactualHash = BLL_Recursos.EncriptarContraseña(claveactual);

            string clavenuevalHash = BLL_Recursos.EncriptarContraseña(nuevaclave);

            if (usu.Password != claveactualHash)
            {
                TempData["Id_Usuario"] = idUsuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña Actual no es correcta";
                return View();
            }
            else if (usu.Password == clavenuevalHash)
            {
                TempData["Id_Usuario"] = idUsuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña nueva no puede ser igual a la clave actual";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["Id_Usuario"] = idUsuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            string mensaje = string.Empty;

            int respuesta = new BLL_Login().CambiarClave(idUsuario, clavenuevalHash, out mensaje);

            if (respuesta == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Id_Usuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();

            }
        }

        public ActionResult CerrarSession()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }


        private List<Usuario> ConvertirDataTableALista(DataTable dt)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (DataRow row in dt.Rows)
            {
                usuarios.Add(new Usuario
                {
                    Correo = row["Correo"].ToString(),
                    Password = row["Password"].ToString(),
                    Restablecer = Convert.ToBoolean(row["Restablecer"].ToString()),
                    Id_Usuario = Convert.ToInt32(row["Id_Usuario"])
                }); ;
            }
            return usuarios;
        }
    }
}