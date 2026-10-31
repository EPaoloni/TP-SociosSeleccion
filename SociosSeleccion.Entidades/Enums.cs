using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SociosSeleccion.Entidades;

public enum CategoriaSocio
{
    [Display(Name = "Activo")]
    Activo = 1,

    [Display(Name = "Adherente")]
    Adherente = 2,

    [Display(Name = "Menor")]
    Menor = 3,

    [Display(Name = "Vitalicio")]
    Vitalicio = 4
}

public enum SectorEstadio
{
    [Display(Name = "Popular")]
    Popular = 1,

    [Display(Name = "Platea baja")]
    PlateaBaja = 2,

    [Display(Name = "Platea alta")]
    PlateaAlta = 3,

    [Display(Name = "Preferencial")]
    Preferencial = 4
}

public enum RolUsuario
{
    [Display(Name = "Administrador")]
    Administrador = 1,

    [Display(Name = "Socio")]
    Socio = 2
}

public static class EnumExtensions
{
    /// <summary>
    /// Devuelve el texto del atributo [Display] de un valor de enum.
    /// </summary>
    public static string NombreVisible(this Enum? valor)
    {
        if (valor is null) return string.Empty;
        var miembro = valor.GetType().GetMember(valor.ToString()).FirstOrDefault();
        return miembro?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? valor.ToString();
    }
}
