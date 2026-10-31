using SociosSeleccion.Entidades;

namespace SociosSeleccion.Negocio.Servicios;

public interface ISocioServicio
{
    List<Socio> Buscar(SocioFiltro filtro);
    List<Socio> ObtenerTodos();
    Socio? ObtenerPorId(int id);
    void Crear(Socio socio);
    void Modificar(Socio socio);
    void Eliminar(int id);
}

public interface IPartidoServicio
{
    List<Partido> ObtenerTodos();
    Partido? ObtenerPorId(int id);
    void Crear(Partido partido);
    void Modificar(Partido partido);
}

public interface IInscripcionServicio
{
    List<Inscripcion> ObtenerTodas();
    List<Inscripcion> ObtenerPorPartido(int partidoId);
    List<Inscripcion> ObtenerPorSocio(int socioId);
    List<Partido> ObtenerPartidosDisponibles();
    int EntradasDisponibles(Partido partido);
    void Inscribir(Inscripcion inscripcion);
    void Cancelar(int id);
    void CancelarDeSocio(int id, int socioId);
}

public interface IUsuarioServicio
{
    Usuario? Autenticar(string nombreUsuario, string contrasena);
    List<Usuario> ObtenerTodos();
    Usuario Registrar(RegistroUsuario registro);
    void CrearUsuariosIniciales();
}
