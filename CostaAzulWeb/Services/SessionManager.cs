using Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading;

public class SessionManager
{
    private static readonly object _lock = new object();
    private static SessionManager _session;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string SessionKey = "UserSession"; // Clave para almacenar el usuario en la sesión

    public bool SesionIniciada { get; private set; }
    public Usuario Usuario { get; private set; }
    public DateTime FechaInicio { get; private set; }

    // Constructor con inyección de dependencias
    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    // Obtener instancia de la sesión
    public static SessionManager GetInstance(IHttpContextAccessor httpContextAccessor)
    {
        lock (_lock)
        {
            if (_session == null)
            {
                _session = new SessionManager(httpContextAccessor);

                // Intentar cargar datos desde la sesión si están disponibles
                var sessionData = httpContextAccessor.HttpContext.Session.GetString(SessionKey);
                if (!string.IsNullOrEmpty(sessionData))
                {
                    var user = JsonConvert.DeserializeObject<Usuario>(sessionData);
                    _session.Usuario = user;
                    _session.SesionIniciada = true;
                    _session.FechaInicio = DateTime.Now; // Ajustar si se almacena en sesión
                }
            }

            return _session;
        }
    }

    // Obtener el perfil del usuario desde la sesión
    public Usuario GetProfile()
    {
        return Usuario;
    }

    // Iniciar sesión
    public void Login(Usuario usuario)
    {
        if (SesionIniciada)
        {
            throw new Exception("Ya hay una sesión activa.");
        }

        lock (_lock)
        {
            _session = new SessionManager(_httpContextAccessor)
            {
                Usuario = usuario,
                FechaInicio = DateTime.Now,
                SesionIniciada = true
            };

            // Guardar el usuario en la sesión
            var sessionData = JsonConvert.SerializeObject(usuario);
            _httpContextAccessor.HttpContext.Session.SetString(SessionKey, sessionData);
        }
    }

    // Cerrar sesión
    public void Logout()
    {
        lock (_lock)
        {
            if (_session != null && SesionIniciada)
            {
                _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
                _session = null;
                SesionIniciada = false;
            }
            else
            {
                throw new Exception("No hay una sesión activa.");
            }
        }
    }
}
