using SociosSeleccion.Datos.Repositorios;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Negocio.Servicios;

public class InscripcionServicio : IInscripcionServicio
{
    private readonly IInscripcionRepositorio _inscripcionRepositorio;
    private readonly ISocioRepositorio _socioRepositorio;
    private readonly IPartidoRepositorio _partidoRepositorio;

    public InscripcionServicio(
        IInscripcionRepositorio inscripcionRepositorio,
        ISocioRepositorio socioRepositorio,
        IPartidoRepositorio partidoRepositorio)
    {
        _inscripcionRepositorio = inscripcionRepositorio;
        _socioRepositorio = socioRepositorio;
        _partidoRepositorio = partidoRepositorio;
    }

    public List<Inscripcion> ObtenerTodas()
    {
        return _inscripcionRepositorio.ObtenerTodas();
    }

    public List<Inscripcion> ObtenerPorPartido(int partidoId)
    {
        return _inscripcionRepositorio.ObtenerPorPartido(partidoId);
    }

    public List<Inscripcion> ObtenerPorSocio(int socioId)
    {
        return _inscripcionRepositorio.ObtenerPorSocio(socioId);
    }

    public List<Partido> ObtenerPartidosDisponibles()
    {
        return _partidoRepositorio.ObtenerDesde(DateTime.Now);
    }

    public int EntradasDisponibles(Partido partido)
    {
        return (partido.CupoEntradas ?? 0) - _inscripcionRepositorio.EntradasReservadas(partido.Id);
    }

    public void Inscribir(Inscripcion inscripcion)
    {
        var socio = _socioRepositorio.ObtenerPorId(inscripcion.SocioId!.Value)
            ?? throw new NegocioException("El socio seleccionado no existe.", nameof(Inscripcion.SocioId));

        var partido = _partidoRepositorio.ObtenerPorId(inscripcion.PartidoId!.Value)
            ?? throw new NegocioException("El partido seleccionado no existe.", nameof(Inscripcion.PartidoId));

        if (partido.Fecha < DateTime.Now)
            throw new NegocioException("No es posible anotarse a un partido que ya se jugó.", nameof(Inscripcion.PartidoId));

        if (_inscripcionRepositorio.Existe(socio.Id, partido.Id))
            throw new NegocioException($"{socio.NombreCompleto} ya está anotado/a a este partido.", nameof(Inscripcion.PartidoId));

        var disponibles = EntradasDisponibles(partido);
        if (inscripcion.CantidadEntradas > disponibles)
            throw new NegocioException($"Solo quedan {disponibles} entradas disponibles para este partido.", nameof(Inscripcion.CantidadEntradas));

        inscripcion.FechaInscripcion = DateTime.Now;
        _inscripcionRepositorio.Agregar(inscripcion);
    }

    public void Cancelar(int id)
    {
        if (_inscripcionRepositorio.ObtenerPorId(id) is null)
            throw new NegocioException("La inscripción que intenta cancelar no existe.");

        _inscripcionRepositorio.Eliminar(id);
    }

    // Un socio solo puede cancelar sus propias inscripciones
    public void CancelarDeSocio(int id, int socioId)
    {
        var inscripcion = _inscripcionRepositorio.ObtenerPorId(id)
            ?? throw new NegocioException("La inscripción que intenta cancelar no existe.");

        if (inscripcion.SocioId != socioId)
            throw new NegocioException("No puede cancelar la inscripción de otro socio.");

        _inscripcionRepositorio.Eliminar(id);
    }
}
