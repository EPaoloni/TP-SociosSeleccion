namespace SociosSeleccion.Entidades;

/// <summary>
/// Usuario del sistema (para el login). Si el rol es Socio, queda vinculado a su socio.
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }

    public int? SocioId { get; set; }
    public Socio? Socio { get; set; }
}
