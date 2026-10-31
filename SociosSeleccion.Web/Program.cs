using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SociosSeleccion.Negocio;

var builder = WebApplication.CreateBuilder(args);

// Todas las páginas requieren login, salvo las marcadas con [AllowAnonymous]
builder.Services.AddControllersWithViews(opciones => opciones.Filters.Add(new AuthorizeFilter()));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/Account/Login";
        opciones.LogoutPath = "/Account/Logout";
        opciones.AccessDeniedPath = "/Account/AccesoDenegado";
        opciones.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddCapaNegocio(builder.Configuration.GetConnectionString("SociosDb")!);

var app = builder.Build();

app.Services.InicializarSistema();

var cultura = new CultureInfo("es-AR");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
