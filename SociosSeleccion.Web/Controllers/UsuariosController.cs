using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio.Servicios;

namespace SociosSeleccion.Web.Controllers;

[Authorize(Roles = nameof(RolUsuario.Administrador))]
public class UsuariosController : Controller
{
    private readonly IUsuarioServicio _usuarioServicio;

    public UsuariosController(IUsuarioServicio usuarioServicio)
    {
        _usuarioServicio = usuarioServicio;
    }

    // Lista de usuarios con los partidos a los que está inscripto cada uno
    public IActionResult Index()
    {
        return View(_usuarioServicio.ObtenerTodos());
    }
}
