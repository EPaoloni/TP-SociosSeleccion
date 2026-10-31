namespace SociosSeleccion.Negocio;

/// <summary>
/// Error de una regla de negocio. Campo indica qué propiedad lo causó (opcional),
/// para que la capa de presentación lo muestre al lado del input correspondiente.
/// </summary>
public class NegocioException : Exception
{
    public string Campo { get; }

    public NegocioException(string mensaje, string campo = "") : base(mensaje)
    {
        Campo = campo;
    }
}
