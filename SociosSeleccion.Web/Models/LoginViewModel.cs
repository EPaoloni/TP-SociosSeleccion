using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Web.Models;

public class LoginViewModel
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
