using Microsoft.AspNetCore.Authentication.Cookies;
using Pautas.Services.Conection;
using Pautas.Services.ProfesorService;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Habilitar la compilación en tiempo de ejecución para Razor Pages
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Habilitar la respuesta en caché
builder.Services.AddResponseCaching();

// Configurar la sesión con un tiempo de espera de inactividad de 30 minutos
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Configurar la autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Ruta de inicio de sesión
        options.LogoutPath = "/User/Logout"; // Ruta de cierre de sesión
        options.AccessDeniedPath = "/User/AccessDenied"; // Ruta de acceso denegado
    });

// Configurar las políticas de autorización para los diferentes niveles
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Nivel1Policy", policy => policy.RequireRole("Nivel 1"));
//    options.AddPolicy("Nivel2Policy", policy => policy.RequireRole("Nivel 2"));
//    options.AddPolicy("Nivel3Policy", policy => policy.RequireRole("Nivel 3"));
//});

builder.Services.AddScoped<FolderAccessService>(); // Cambia a AddTransient si es más apropiado
builder.Services.AddSingleton<ConnectionDb>(); // Ejemplo de registro para ConnectionDb, ajusta según tu implementación


var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 días. Puedes cambiarlo según tus escenarios de producción.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization(); // Habilitar la autorización

app.UseSession(); // Habilitar la sesión

// Mapear la ruta predeterminada del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
