using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Entidades;

/// <summary>
/// Datos que completa un socio para crear su usuario.
/// Con el N° de socio y el DNI se verifica que realmente sea socio.
/// </summary>
public class RegistroUsuario
{
    [Display(Name = "N° de socio")]
    [Required(ErrorMessage = "El número de socio es obligatorio.")]
    [Range(1, 1000, ErrorMessage = "El número de socio debe estar entre 1 y 1000.")]
    public int? NumeroSocio { get; set; }

    [Display(Name = "DNI")]
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 dígitos, sin puntos.")]
    public string Dni { get; set; } = string.Empty;

    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "El usuario debe tener entre 4 y 30 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._]+$", ErrorMessage = "El usuario solo puede tener letras, números, punto y guion bajo.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;

    [Display(Name = "Repetir contraseña")]
    [Required(ErrorMessage = "Debe repetir la contraseña.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContrasena { get; set; } = string.Empty;
}
