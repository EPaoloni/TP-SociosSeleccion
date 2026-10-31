using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Web.Controllers;

public class HomeController : Controller
{
    // Página de inicio según el rol del usuario
    public IActionResult Index()
    {
        if (User.IsInRole(nameof(RolUsuario.Administrador)))
            return RedirectToAction("Index", "Socios");

        return RedirectToAction("Index", "MisPartidos");
    }

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}
