using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Entidades;

/// <summary>
/// Criterios del buscador de socios.
/// </summary>
public class SocioFiltro
{
    [Display(Name = "Apellido o nombre")]
    public string? Apellido { get; set; }

    [Display(Name = "DNI")]
    public string? Dni { get; set; }

    [Display(Name = "Categoría")]
    public CategoriaSocio? Categoria { get; set; }
}
