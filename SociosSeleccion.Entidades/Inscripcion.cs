using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Entidades;

/// <summary>
/// Entidad de relación: inscripción de un socio para ir a un partido.
/// </summary>
public class Inscripcion
{
    public int Id { get; set; }

    [Display(Name = "Socio")]
    [Required(ErrorMessage = "Debe seleccionar un socio.")]
    public int? SocioId { get; set; }

    public Socio? Socio { get; set; }

    [Display(Name = "Partido")]
    [Required(ErrorMessage = "Debe seleccionar un partido.")]
    public int? PartidoId { get; set; }

    public Partido? Partido { get; set; }

    [Display(Name = "Sector")]
    [Required(ErrorMessage = "Debe seleccionar un sector.")]
    public SectorEstadio? Sector { get; set; }

    [Display(Name = "Cantidad de entradas")]
    [Required(ErrorMessage = "La cantidad de entradas es obligatoria.")]
    [Range(1, 4, ErrorMessage = "Se pueden solicitar entre 1 y 4 entradas.")]
    public int? CantidadEntradas { get; set; }

    [Display(Name = "Fecha de inscripción")]
    public DateTime FechaInscripcion { get; set; }
}
