using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SociosSeleccion.Entidades;
using SociosSeleccion.Negocio;
using SociosSeleccion.Negocio.Servicios;
using SociosSeleccion.Web.Models;

namespace SociosSeleccion.Web.Controllers;

[Authorize(Roles = nameof(RolUsuario.Administrador))]
public class SociosController : Controller
{
    private readonly ISocioServicio _socioServicio;

    public SociosController(ISocioServicio socioServicio)
    {
        _socioServicio = socioServicio;
    }

    // GET: /Socios  (listado + buscador)
    public IActionResult Index(SocioFiltro filtro)
    {
        var modelo = new SocioIndexViewModel
        {
            Filtro = filtro,
            Socios = _socioServicio.Buscar(filtro)
        };
        return View(modelo);
    }

    public IActionResult Details(int id)
    {
        var socio = _socioServicio.ObtenerPorId(id);
        if (socio is null) return NotFound();

        return View(socio);
    }

    // ---------- Alta ----------
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Socio { FechaAlta = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Socio socio)
    {
        if (!ModelState.IsValid) return View(socio);

        try
        {
            _socioServicio.Crear(socio);
        }
        catch (NegocioException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(socio);
        }

        TempData["Mensaje"] = $"Socio {socio.NombreCompleto} dado de alta correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- Modificación ----------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var socio = _socioServicio.ObtenerPorId(id);
        if (socio is null) return NotFound();

        return View(socio);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Socio socio)
    {
        if (id != socio.Id) return BadRequest();
        if (!ModelState.IsValid) return View(socio);

        try
        {
            _socioServicio.Modificar(socio);
        }
        catch (NegocioException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(socio);
        }

        TempData["Mensaje"] = $"Socio {socio.NombreCompleto} modificado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- Baja ----------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var socio = _socioServicio.ObtenerPorId(id);
        if (socio is null) return NotFound();

        return View(socio);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _socioServicio.Eliminar(id);
            TempData["Mensaje"] = "Socio eliminado correctamente.";
        }
        catch (NegocioException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
