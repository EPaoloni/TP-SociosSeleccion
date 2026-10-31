using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos.Repositorios;

public interface ISocioRepositorio
{
    List<Socio> Buscar(SocioFiltro filtro);
    List<Socio> ObtenerTodos();
    Socio? ObtenerPorId(int id);
    bool ExisteNumeroSocio(int numeroSocio, int idExcluido);
    bool ExisteDni(string dni, int idExcluido);
    void Agregar(Socio socio);
    void Actualizar(Socio socio);
    void Eliminar(int id);
}

public interface IPartidoRepositorio
{
    List<Partido> ObtenerTodos();
    List<Partido> ObtenerDesde(DateTime fecha);
    Partido? ObtenerPorId(int id);
    bool ExisteEnElDia(DateTime fecha, int idExcluido);
    void Agregar(Partido partido);
    void Actualizar(Partido partido);
}

public interface IInscripcionRepositorio
{
    List<Inscripcion> ObtenerTodas();
    List<Inscripcion> ObtenerPorPartido(int partidoId);
    List<Inscripcion> ObtenerPorSocio(int socioId);
    Inscripcion? ObtenerPorId(int id);
    bool Existe(int socioId, int partidoId);
    int EntradasReservadas(int partidoId);
    void Agregar(Inscripcion inscripcion);
    void Eliminar(int id);
}

public interface IUsuarioRepositorio
{
    List<Usuario> ObtenerTodos();
    Usuario? ObtenerPorNombreUsuario(string nombreUsuario);
    bool ExisteParaSocio(int socioId);
    bool HayUsuarios();
    void Agregar(Usuario usuario);
}
