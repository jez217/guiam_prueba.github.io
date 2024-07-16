using Microsoft.AspNetCore.Authentication.Cookies;
using Pautas.Services.Conection;
using Pautas.Services.ProfesorService;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Habilitar la compilaci�n en tiempo de ejecuci�n para Razor Pages
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Habilitar la respuesta en cach�
builder.Services.AddResponseCaching();

// Configurar la sesi�n con un tiempo de espera de inactividad de 30 minutos
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Configurar la autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Ruta de inicio de sesi�n
        options.LogoutPath = "/User/Logout"; // Ruta de cierre de sesi�n
        options.AccessDeniedPath = "/User/AccessDenied"; // Ruta de acceso denegado
    });

// Configurar las pol�ticas de autorizaci�n para los diferentes niveles
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Nivel1Policy", policy => policy.RequireRole("Nivel 1"));
//    options.AddPolicy("Nivel2Policy", policy => policy.RequireRole("Nivel 2"));
//    options.AddPolicy("Nivel3Policy", policy => policy.RequireRole("Nivel 3"));
//});

builder.Services.AddScoped<FolderAccessService>(); // Cambia a AddTransient si es m�s apropiado
builder.Services.AddSingleton<ConnectionDb>(); // Ejemplo de registro para ConnectionDb, ajusta seg�n tu implementaci�n


var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 d�as. Puedes cambiarlo seg�n tus escenarios de producci�n.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticaci�n
app.UseAuthorization(); // Habilitar la autorizaci�n

app.UseSession(); // Habilitar la sesi�n

// Mapear la ruta predeterminada del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
