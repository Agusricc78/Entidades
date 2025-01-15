using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CostaAzulWeb.Filtros
{
    public class LoginStatusFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginStatusFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is FileResult)
            {
                return;
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("SesionIniciada") == "true";

            // Establecer un valor en Items (una propiedad compartida durante la solicitud)
            context.HttpContext.Items["IsLoggedIn"] = isLoggedIn;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Recuperar el valor de la sesión
            var isLoggedIn = _httpContextAccessor.HttpContext.Session.GetString("SesionIniciada") == "true";

            // Asignar al ViewBag
            context.HttpContext.Items["IsLoggedIn"] = isLoggedIn;

            // También puedes establecer otros valores como el UserId
            if (isLoggedIn)
            {
                var usuarioJson = _httpContextAccessor.HttpContext.Session.GetString("Usuario");
                if (!string.IsNullOrEmpty(usuarioJson))
                {
                    var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                    context.HttpContext.Items["UserId"] = usuario.Id_Usuario;
                }
            }

            // Continuar con la ejecución de la acción
            await next();
        }

    }
}
