using SociosSeleccion.Datos.Repositorios;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Negocio.Servicios;

public class PartidoServicio : IPartidoServicio
{
    private readonly IPartidoRepositorio _partidoRepositorio;
    private readonly IInscripcionRepositorio _inscripcionRepositorio;

    public PartidoServicio(IPartidoRepositorio partidoRepositorio, IInscripcionRepositorio inscripcionRepositorio)
    {
        _partidoRepositorio = partidoRepositorio;
        _inscripcionRepositorio = inscripcionRepositorio;
    }

    public List<Partido> ObtenerTodos()
    {
        return _partidoRepositorio.ObtenerTodos();
    }

    public Partido? ObtenerPorId(int id)
    {
        return _partidoRepositorio.ObtenerPorId(id);
    }

    public void Crear(Partido partido)
    {
        Normalizar(partido);
        Validar(partido);
        _partidoRepositorio.Agregar(partido);
    }

    public void Modificar(Partido partido)
    {
        var existente = _partidoRepositorio.ObtenerPorId(partido.Id)
            ?? throw new NegocioException("El partido que intenta modificar no existe.");

        if (existente.Fecha <= DateTime.Now)
            throw new NegocioException("No se puede modificar un partido que ya se jugó.");

        Normalizar(partido);
        Validar(partido);

        var reservadas = _inscripcionRepositorio.EntradasReservadas(partido.Id);
        if (partido.CupoEntradas < reservadas)
            throw new NegocioException($"El cupo no puede ser menor a las {reservadas} entradas ya reservadas por los socios.", nameof(Partido.CupoEntradas));

        _partidoRepositorio.Actualizar(partido);
    }

    private static void Normalizar(Partido partido)
    {
        partido.Rival = partido.Rival.Trim();
        partido.Estadio = partido.Estadio.Trim();
    }

    private void Validar(Partido partido)
    {
        if (!Competiciones.Todas.Contains(partido.Competicion))
            throw new NegocioException("La competición seleccionada no es válida.", nameof(Partido.Competicion));

        if (partido.Fecha <= DateTime.Now)
            throw new NegocioException("La fecha del partido tiene que ser posterior a hoy.", nameof(Partido.Fecha));

        if (partido.Fecha > DateTime.Today.AddYears(5))
            throw new NegocioException("La fecha del partido no puede ser a más de 5 años.", nameof(Partido.Fecha));

        if (partido.Rival.Equals("Argentina", StringComparison.OrdinalIgnoreCase))
            throw new NegocioException("El rival no puede ser Argentina.", nameof(Partido.Rival));

        if (_partidoRepositorio.ExisteEnElDia(partido.Fecha!.Value, partido.Id))
            throw new NegocioException("Ya hay otro partido cargado para ese día.", nameof(Partido.Fecha));
    }
}
