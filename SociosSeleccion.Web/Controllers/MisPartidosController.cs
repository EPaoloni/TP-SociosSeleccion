using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio;
using SociosSeleccion.Negocio.Servicios;

namespace SociosSeleccion.Web.Controllers;

/// <summary>
/// Pantallas del socio logueado: sus partidos, anotarse y cancelar.
/// </summary>
[Authorize(Roles = nameof(RolUsuario.Socio))]
public class MisPartidosController : Controller
{
    private readonly IInscripcionServicio _inscripcionServicio;

    public MisPartidosController(IInscripcionServicio inscripcionServicio)
    {
        _inscripcionServicio = inscripcionServicio;
    }

    private int SocioIdActual => int.Parse(User.FindFirst("SocioId")!.Value);

    public IActionResult Index()
    {
        return View(_inscripcionServicio.ObtenerPorSocio(SocioIdActual));
    }

    [HttpGet]
    public IActionResult Create(int? partidoId)
    {
        CargarPartidos();
        return View(new Inscripcion { PartidoId = partidoId, CantidadEntradas = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Inscripcion inscripcion)
    {
        // El socio siempre es el usuario logueado, no se toma del formulario
        inscripcion.SocioId = SocioIdActual;
        ModelState.Remove(nameof(Inscripcion.SocioId));

        if (ModelState.IsValid)
        {
            try
            {
                _inscripcionServicio.Inscribir(inscripcion);
                TempData["Mensaje"] = "¡Listo! Quedaste anotado/a al partido.";
                return RedirectToAction(nameof(Index));
            }
            catch (NegocioException ex)
            {
                ModelState.AddModelError(ex.Campo, ex.Message);
            }
        }

        CargarPartidos();
        return View(inscripcion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancelar(int id)
    {
        try
        {
            _inscripcionServicio.CancelarDeSocio(id, SocioIdActual);
            TempData["Mensaje"] = "Inscripción cancelada.";
        }
        catch (NegocioException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private void CargarPartidos()
    {
        var partidos = _inscripcionServicio.ObtenerPartidosDisponibles()
            .Select(p => new { p.Id, Texto = $"{p.Descripcion} - {p.Competicion} ({_inscripcionServicio.EntradasDisponibles(p)} entradas disp.)" });
        ViewBag.Partidos = new SelectList(partidos, "Id", "Texto");
    }
}
