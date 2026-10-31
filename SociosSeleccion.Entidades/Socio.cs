using System.ComponentModel.DataAnnotations;
using SociosSeleccion.Entidades.Validaciones;

namespace SociosSeleccion.Entidades;

/// <summary>
/// Entidad principal: socio de la Selección Argentina.
/// </summary>
public class Socio
{
    public int Id { get; set; }

    // Campo numérico con rango 1 a 1000
    [Display(Name = "N° de socio")]
    [Required(ErrorMessage = "El número de socio es obligatorio.")]
    [Range(1, 1000, ErrorMessage = "El número de socio debe estar entre 1 y 1000.")]
    public int? NumeroSocio { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Display(Name = "DNI")]
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 dígitos, sin puntos.")]
    public string Dni { get; set; } = string.Empty;

    // Campo fecha (validado con atributo propio)
    [Display(Name = "Fecha de nacimiento")]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [FechaValida(AnioMinimo = 1900)]
    public DateTime? FechaNacimiento { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^[0-9\s\-\+\(\)]{8,20}$", ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres (números, espacios, +, -, paréntesis).")]
    public string Telefono { get; set; } = string.Empty;

    [Display(Name = "Domicilio")]
    [Required(ErrorMessage = "El domicilio es obligatorio.")]
    [StringLength(120, MinimumLength = 5, ErrorMessage = "El domicilio debe tener entre 5 y 120 caracteres.")]
    public string Domicilio { get; set; } = string.Empty;

    // Campo tipo lista (combo)
    [Display(Name = "Provincia")]
    [Required(ErrorMessage = "Debe seleccionar una provincia.")]
    public string Provincia { get; set; } = string.Empty;

    // Campo tipo lista (combo)
    [Display(Name = "Categoría")]
    [Required(ErrorMessage = "Debe seleccionar una categoría.")]
    public CategoriaSocio? Categoria { get; set; }

    [Display(Name = "Fecha de alta")]
    [Required(ErrorMessage = "La fecha de alta es obligatoria.")]
    [DataType(DataType.Date)]
    [FechaValida(AnioMinimo = 1990)]
    public DateTime? FechaAlta { get; set; }

    // Entidad de relación
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

    public string NombreCompleto => $"{Apellido}, {Nombre}";

    public int? Edad
    {
        get
        {
            if (FechaNacimiento is null) return null;
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Value.Year;
            if (FechaNacimiento.Value.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}
