using BusinessLogicLayer;
using Entities;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configuración para usar memoria distribuida (requerido para sesiones)
builder.Services.AddDistributedMemoryCache();

// Configuración de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true;                // Seguridad contra scripts maliciosos
    options.Cookie.IsEssential = true;             // Esencial para la funcionalidad de la aplicación
});

// Configuración para cookies personalizadas
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "UserSessionCookie";
    options.LoginPath = "/Login";                   // Ruta al iniciar sesión
    options.LogoutPath = "/Logout";                 // Ruta al cerrar sesión
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tiempo de expiración de la cookie
    options.AccessDeniedPath = "/AccessDenied";     // Ruta si el usuario no tiene permisos
});

builder.Services.AddHttpContextAccessor();
// Requerido para acceder a HttpContext en servicios o controladores

// Registro de dependencias
builder.Services.AddScoped<BLL_Login>(); // Registro de la clase BLL_Login
builder.Services.AddScoped<BLL_Productos>(); // Registro de la clase BLL_Productos
builder.Services.AddScoped<BLL_Categorias>(); // Registro de la clase BLL_Categorias

// Configuración para capturar la IP del usuario detrás de un proxy o balanceador de carga
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear(); // Opcional, especificar proxies conocidos si aplica
});

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Seguridad HTTPS
}

app.UseHttpsRedirection(); // Redirigir HTTP a HTTPS
app.UseStaticFiles();       // Servir archivos estáticos (CSS, JS, imágenes)

// Habilitar el middleware para procesar encabezados de proxies
app.UseForwardedHeaders();

app.UseRouting();           // Configurar el enrutamiento

app.UseSession();           // Habilitar el middleware de sesión
app.UseAuthorization();
// Habilitar autorización

// Configuración de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
