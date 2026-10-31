using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio;
using SociosSeleccion.Negocio.Servicios;

namespace SociosSeleccion.Web.Controllers;

[Authorize(Roles = nameof(RolUsuario.Administrador))]
public class InscripcionesController : Controller
{
    private readonly IInscripcionServicio _inscripcionServicio;
    private readonly ISocioServicio _socioServicio;

    public InscripcionesController(IInscripcionServicio inscripcionServicio, ISocioServicio socioServicio)
    {
        _inscripcionServicio = inscripcionServicio;
        _socioServicio = socioServicio;
    }

    public IActionResult Index()
    {
        return View(_inscripcionServicio.ObtenerTodas());
    }

    // ---------- Alta de inscripción asociada a un socio ----------
    [HttpGet]
    public IActionResult Create(int? socioId)
    {
        CargarCombos();
        return View(new Inscripcion { SocioId = socioId, CantidadEntradas = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Inscripcion inscripcion)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _inscripcionServicio.Inscribir(inscripcion);
                TempData["Mensaje"] = "Inscripción registrada correctamente.";
                return RedirectToAction("Details", "Socios", new { id = inscripcion.SocioId });
            }
            catch (NegocioException ex)
            {
                ModelState.AddModelError(ex.Campo, ex.Message);
            }
        }

        CargarCombos();
        return View(inscripcion);
    }

    // ---------- Cancelar inscripción ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancelar(int id, int? socioId)
    {
        try
        {
            _inscripcionServicio.Cancelar(id);
            TempData["Mensaje"] = "Inscripción cancelada.";
        }
        catch (NegocioException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return socioId.HasValue
            ? RedirectToAction("Details", "Socios", new { id = socioId })
            : RedirectToAction(nameof(Index));
    }

    private void CargarCombos()
    {
        var socios = _socioServicio.ObtenerTodos()
            .Select(s => new { s.Id, Texto = $"N° {s.NumeroSocio} - {s.NombreCompleto} (DNI {s.Dni})" });
        ViewBag.Socios = new SelectList(socios, "Id", "Texto");

        var partidos = _inscripcionServicio.ObtenerPartidosDisponibles()
            .Select(p => new { p.Id, Texto = $"{p.Descripcion} - {p.Competicion} ({_inscripcionServicio.EntradasDisponibles(p)} entradas disp.)" });
        ViewBag.Partidos = new SelectList(partidos, "Id", "Texto");
    }
}
