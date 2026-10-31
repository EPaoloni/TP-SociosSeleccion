namespace SociosSeleccion.Entidades;

/// <summary>
/// Opciones del combo de competiciones de un partido.
/// </summary>
public static class Competiciones
{
    public static readonly IReadOnlyList<string> Todas = new[]
    {
        "Eliminatorias Sudamericanas",
        "Copa América",
        "Copa del Mundo",
        "Finalissima",
        "Amistoso internacional"
    };
}
