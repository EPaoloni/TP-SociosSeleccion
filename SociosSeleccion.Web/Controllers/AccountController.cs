using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio;
using SociosSeleccion.Negocio.Servicios;
using SociosSeleccion.Web.Models;

namespace SociosSeleccion.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUsuarioServicio _usuarioServicio;

    public AccountController(IUsuarioServicio usuarioServicio)
    {
        _usuarioServicio = usuarioServicio;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo)
    {
        if (!ModelState.IsValid) return View(modelo);

        var usuario = _usuarioServicio.Autenticar(modelo.NombreUsuario, modelo.Contrasena);
        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View(modelo);
        }

        await IniciarSesion(usuario);

        if (!string.IsNullOrEmpty(modelo.ReturnUrl) && Url.IsLocalUrl(modelo.ReturnUrl))
            return Redirect(modelo.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    // ---------- Registro de socios ----------
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Registro()
    {
        return View(new RegistroUsuario());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroUsuario registro)
    {
        if (!ModelState.IsValid) return View(registro);

        Usuario usuario;
        try
        {
            usuario = _usuarioServicio.Registrar(registro);
        }
        catch (NegocioException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(registro);
        }

        await IniciarSesion(usuario);
        TempData["Mensaje"] = $"¡Bienvenido/a, {usuario.NombreCompleto}! Tu usuario se creó correctamente.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    private async Task IniciarSesion(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario),
            new(ClaimTypes.Role, usuario.Rol.ToString()),
            new("NombreCompleto", usuario.NombreCompleto)
        };
        if (usuario.SocioId.HasValue)
            claims.Add(new Claim("SocioId", usuario.SocioId.Value.ToString()));

        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));
    }
}
