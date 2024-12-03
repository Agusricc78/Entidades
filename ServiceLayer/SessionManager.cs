using DataAccessLayer;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceLayer
{
    public class SessionManager
    {
        private static object _lock = new object();
        private static SessionManager _session;
        private static string CookieName = "UserSessionCookie";  // Nombre de la cookie

        public bool SesionIniciada { get; private set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaInicio { get; set; }

        // Propiedad estática para obtener la instancia de la sesión
        public static SessionManager GetInstance
        {
            get
            {
                // Primero, verificamos si la cookie está presente
                var cookie = HttpContext.Current.Request.Cookies[CookieName];

                if (cookie != null)
                {
                    // Si la cookie existe, deserializamos la información de la sesión
                    var user = DeserializeCookie(cookie.Value);
                    _session = new SessionManager
                    {
                        Usuario = user,
                        SesionIniciada = true,
                        FechaInicio = DateTime.Now
                    };
                    return _session;
                }
                else
                {
                    // Si la cookie no existe, significa que la sesión no ha sido iniciada
                    return null;
                }
            }
        }


        // Método para obtener el perfil del usuario desde la sesión
        public static Usuario GetProfile()
        {
            return _session?.Usuario;
        }

        // Método para iniciar sesión
        public void Login(string Nombre)
        {
            if (SesionIniciada)
            {
                throw new Exception("Ya hay una sesión activa");
            }

            var user = new Mp_User().ObtenerUsuarioXNom(Nombre);

            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                    _session.Usuario = user;
                    _session.FechaInicio = DateTime.Now;
                    _session.SesionIniciada = true;

                    // Crear la cookie con los datos del usuario
                    var cookieValue = SerializeCookie(user);
                    var cookie = new HttpCookie(CookieName, cookieValue)
                    {
                        Expires = DateTime.Now.AddHours(1)  // Duración de la cookie
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    throw new Exception("Sesión ya iniciada");
                }
            }
        }

        // Método para cerrar sesión y eliminar la cookie
        public static void Logout()
        {
            lock (_lock)
            {
                if (_session != null)
                {
                    _session = null;
                    var cookie = HttpContext.Current.Request.Cookies[CookieName];
                    if (cookie != null)
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);  // Expirar la cookie
                        HttpContext.Current.Response.Cookies.Add(cookie);  // Eliminar la cookie
                    }
                }
                else
                {
                    throw new Exception("Sesión no iniciada");
                }
            }
        }

        // Serializar los datos del usuario a un string que se puede almacenar en la cookie
        private static string SerializeCookie(Usuario user)
        {
            var json = JsonConvert.SerializeObject(user);  // Usando JSON para serializar
            return json;
        }

        // Deserializar los datos de la cookie de vuelta a un objeto Usuario
        private static Usuario DeserializeCookie(string cookieValue)
        {
            return JsonConvert.DeserializeObject<Usuario>(cookieValue);
        }
    }




}
