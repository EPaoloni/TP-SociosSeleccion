namespace SociosSeleccion.Entidades;

/// <summary>
/// Opciones del combo de provincias.
/// </summary>
public static class Provincias
{
    public static readonly IReadOnlyList<string> Todas = new[]
    {
        "Buenos Aires", "CABA", "Catamarca", "Chaco", "Chubut", "Córdoba",
        "Corrientes", "Entre Ríos", "Formosa", "Jujuy", "La Pampa", "La Rioja",
        "Mendoza", "Misiones", "Neuquén", "Río Negro", "Salta", "San Juan",
        "San Luis", "Santa Cruz", "Santa Fe", "Santiago del Estero",
        "Tierra del Fuego", "Tucumán", "Exterior"
    };
}
