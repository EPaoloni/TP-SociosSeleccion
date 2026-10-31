using SociosSeleccion.Entidades;

namespace SociosSeleccion.Web.Models;

public class SocioIndexViewModel
{
    public SocioFiltro Filtro { get; set; } = new();
    public List<Socio> Socios { get; set; } = new();
}
