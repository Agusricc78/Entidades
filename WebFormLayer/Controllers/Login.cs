using BusinessLogicLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebFormLayer.Controllers
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica si el usuario ya está autenticado
            if (Session["Usuario"] != null)
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = username.Text;
            string password = password.Text;

            // Utilizamos la instancia de LoginService para validar el login
            var loginService = new LoginService();
            Usuario user = loginService.ValidateLogin(username, password);

            if (user != null)
            {
                // Guardar sesión y redirigir
                Session["Usuario"] = user;
                Response.Redirect("Home.aspx");
            }
            else
            {
                // Mostrar mensaje de error
                Response.Write("<script>alert('Usuario o contraseña incorrectos');</script>");
            }
        }
    }
}