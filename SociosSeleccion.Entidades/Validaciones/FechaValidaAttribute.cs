using System.ComponentModel.DataAnnotations;

namespace SociosSeleccion.Entidades.Validaciones;

/// <summary>
/// Valida que una fecha no sea futura ni anterior a un año mínimo.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FechaValidaAttribute : ValidationAttribute
{
    public int AnioMinimo { get; set; } = 1900;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Si viene vacía la controla [Required]
        if (value is null) return ValidationResult.Success;

        if (value is not DateTime fecha)
            return new ValidationResult($"El campo {validationContext.DisplayName} no es una fecha válida.");

        if (fecha.Year < AnioMinimo)
            return new ValidationResult($"El campo {validationContext.DisplayName} no puede ser anterior al año {AnioMinimo}.");

        if (fecha.Date > DateTime.Today)
            return new ValidationResult($"El campo {validationContext.DisplayName} no puede ser una fecha futura.");

        return ValidationResult.Success;
    }
}
