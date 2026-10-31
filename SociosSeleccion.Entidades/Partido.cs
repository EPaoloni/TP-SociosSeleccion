using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Entidades;

/// <summary>
/// Partido de la Selección al que los socios pueden anotarse.
/// </summary>
public class Partido
{
    public int Id { get; set; }

    [Display(Name = "Rival")]
    [Required(ErrorMessage = "El rival es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El rival debe tener entre 2 y 50 caracteres.")]
    public string Rival { get; set; } = string.Empty;

    [Display(Name = "Fecha y hora")]
    [Required(ErrorMessage = "La fecha y hora del partido es obligatoria.")]
    [DataType(DataType.DateTime)]
    public DateTime? Fecha { get; set; }

    [Display(Name = "Estadio")]
    [Required(ErrorMessage = "El estadio es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El estadio debe tener entre 3 y 80 caracteres.")]
    public string Estadio { get; set; } = string.Empty;

    [Display(Name = "Competición")]
    [Required(ErrorMessage = "Debe seleccionar una competición.")]
    public string Competicion { get; set; } = string.Empty;

    [Display(Name = "Cupo de entradas")]
    [Required(ErrorMessage = "El cupo de entradas es obligatorio.")]
    [Range(1, 100000, ErrorMessage = "El cupo debe estar entre 1 y 100.000 entradas.")]
    public int? CupoEntradas { get; set; }

    public string Descripcion => $"Argentina vs {Rival} - {Fecha:dd/MM/yyyy HH:mm} - {Estadio}";
}
