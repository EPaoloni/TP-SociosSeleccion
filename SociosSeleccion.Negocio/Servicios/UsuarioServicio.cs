using System.Security.Cryptography;
using SociosSeleccion.Datos.Repositorios;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Negocio.Servicios;

public class UsuarioServicio : IUsuarioServicio
{
    private const int Iteraciones = 100_000;
    private const int TamanioSalt = 16;
    private const int TamanioHash = 32;

    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly ISocioRepositorio _socioRepositorio;

    public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio, ISocioRepositorio socioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _socioRepositorio = socioRepositorio;
    }

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        var usuario = _usuarioRepositorio.ObtenerPorNombreUsuario(nombreUsuario.Trim().ToLower());
        if (usuario is null) return null;

        return VerificarContrasena(contrasena, usuario.PasswordHash) ? usuario : null;
    }

    public List<Usuario> ObtenerTodos()
    {
        return _usuarioRepositorio.ObtenerTodos();
    }

    public Usuario Registrar(RegistroUsuario registro)
    {
        var nombreUsuario = registro.NombreUsuario.Trim().ToLower();

        // Se verifica que los datos correspondan a un socio real
        var socio = _socioRepositorio
            .Buscar(new SocioFiltro { Dni = registro.Dni.Trim() })
            .FirstOrDefault(s => s.Dni == registro.Dni.Trim() && s.NumeroSocio == registro.NumeroSocio);

        if (socio is null)
            throw new NegocioException("El N° de socio y el DNI no corresponden a ningún socio registrado.");

        if (_usuarioRepositorio.ExisteParaSocio(socio.Id))
            throw new NegocioException("Este socio ya tiene un usuario creado.");

        if (_usuarioRepositorio.ObtenerPorNombreUsuario(nombreUsuario) is not null)
            throw new NegocioException("El nombre de usuario ya está en uso, elija otro.", nameof(RegistroUsuario.NombreUsuario));

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario,
            NombreCompleto = $"{socio.Nombre} {socio.Apellido}",
            PasswordHash = HashearContrasena(registro.Contrasena),
            Rol = RolUsuario.Socio,
            SocioId = socio.Id
        };
        _usuarioRepositorio.Agregar(usuario);
        return usuario;
    }

    // Usuarios de prueba: el administrador y un socio (vinculado al socio de ejemplo 5)
    public void CrearUsuariosIniciales()
    {
        if (_usuarioRepositorio.HayUsuarios()) return;

        _usuarioRepositorio.Agregar(new Usuario
        {
            NombreUsuario = "admin",
            NombreCompleto = "Administrador",
            PasswordHash = HashearContrasena("1234"),
            Rol = RolUsuario.Administrador
        });

        _usuarioRepositorio.Agregar(new Usuario
        {
            NombreUsuario = "juan.perez",
            NombreCompleto = "Juan Pérez",
            PasswordHash = HashearContrasena("1234"),
            Rol = RolUsuario.Socio,
            SocioId = 5
        });
    }

    // Guarda "salt.hash" en Base64 usando PBKDF2 (nunca la contraseña en texto plano)
    private static string HashearContrasena(string contrasena)
    {
        var salt = RandomNumberGenerator.GetBytes(TamanioSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, salt, Iteraciones, HashAlgorithmName.SHA256, TamanioHash);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static bool VerificarContrasena(string contrasena, string almacenado)
    {
        var partes = almacenado.Split('.');
        if (partes.Length != 2) return false;

        var salt = Convert.FromBase64String(partes[0]);
        var hashEsperado = Convert.FromBase64String(partes[1]);
        var hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, salt, Iteraciones, HashAlgorithmName.SHA256, TamanioHash);

        return CryptographicOperations.FixedTimeEquals(hash, hashEsperado);
    }
}
