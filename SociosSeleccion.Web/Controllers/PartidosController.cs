using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio;
using SociosSeleccion.Negocio.Servicios;
using SociosSeleccion.Web.Models;

namespace SociosSeleccion.Web.Controllers;

/// <summary>
/// Pantalla de partidos: la ven el administrador y los socios.
/// Solo el administrador puede cargar y modificar partidos.
/// </summary>
public class PartidosController : Controller
{
    private readonly IPartidoServicio _partidoServicio;
    private readonly IInscripcionServicio _inscripcionServicio;

    public PartidosController(IPartidoServicio partidoServicio, IInscripcionServicio inscripcionServicio)
    {
        _partidoServicio = partidoServicio;
        _inscripcionServicio = inscripcionServicio;
    }

    public IActionResult Index()
    {
        var partidos = _partidoServicio.ObtenerTodos()
            .Select(p => CrearResumen(p.Id))
            .ToList();

        return View(partidos);
    }

    public IActionResult Details(int id)
    {
        if (_partidoServicio.ObtenerPorId(id) is null) return NotFound();

        return View(CrearResumen(id));
    }

    // ---------- Alta ----------
    [Authorize(Roles = nameof(RolUsuario.Administrador))]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Partido { Fecha = DateTime.Today.AddDays(7).AddHours(21) });
    }

    [Authorize(Roles = nameof(RolUsuario.Administrador))]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Partido partido)
    {
        if (!ModelState.IsValid) return View(partido);

        try
        {
            _partidoServicio.Crear(partido);
        }
        catch (NegocioException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(partido);
        }

        TempData["Mensaje"] = $"Partido Argentina vs {partido.Rival} cargado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- Modificación ----------
    [Authorize(Roles = nameof(RolUsuario.Administrador))]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var partido = _partidoServicio.ObtenerPorId(id);
        if (partido is null) return NotFound();

        return View(partido);
    }

    [Authorize(Roles = nameof(RolUsuario.Administrador))]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Partido partido)
    {
        if (id != partido.Id) return BadRequest();
        if (!ModelState.IsValid) return View(partido);

        try
        {
            _partidoServicio.Modificar(partido);
        }
        catch (NegocioException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(partido);
        }

        TempData["Mensaje"] = $"Partido Argentina vs {partido.Rival} modificado correctamente.";
        return RedirectToAction(nameof(Details), new { id = partido.Id });
    }

    private PartidoResumen CrearResumen(int partidoId)
    {
        var partido = _partidoServicio.ObtenerPorId(partidoId)!;
        var inscripciones = _inscripcionServicio.ObtenerPorPartido(partidoId);

        return new PartidoResumen
        {
            Partido = partido,
            Inscripciones = inscripciones,
            EntradasDisponibles = _inscripcionServicio.EntradasDisponibles(partido)
        };
    }
}
