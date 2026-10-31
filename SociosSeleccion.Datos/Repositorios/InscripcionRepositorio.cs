using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos.Repositorios;

public class InscripcionRepositorio : IInscripcionRepositorio
{
    private readonly AppDbContext _context;

    public InscripcionRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public List<Inscripcion> ObtenerTodas()
    {
        return _context.Inscripciones
            .Include(i => i.Socio)
            .Include(i => i.Partido)
            .AsNoTracking()
            .OrderBy(i => i.Partido!.Fecha)
            .ThenBy(i => i.Socio!.Apellido)
            .ToList();
    }

    public List<Inscripcion> ObtenerPorPartido(int partidoId)
    {
        return _context.Inscripciones
            .Include(i => i.Socio)
            .AsNoTracking()
            .Where(i => i.PartidoId == partidoId)
            .OrderBy(i => i.Socio!.Apellido)
            .ThenBy(i => i.Socio!.Nombre)
            .ToList();
    }

    public List<Inscripcion> ObtenerPorSocio(int socioId)
    {
        return _context.Inscripciones
            .Include(i => i.Partido)
            .AsNoTracking()
            .Where(i => i.SocioId == socioId)
            .OrderBy(i => i.Partido!.Fecha)
            .ToList();
    }

    public Inscripcion? ObtenerPorId(int id)
    {
        return _context.Inscripciones.AsNoTracking().FirstOrDefault(i => i.Id == id);
    }

    public bool Existe(int socioId, int partidoId)
    {
        return _context.Inscripciones.Any(i => i.SocioId == socioId && i.PartidoId == partidoId);
    }

    public int EntradasReservadas(int partidoId)
    {
        return _context.Inscripciones
            .Where(i => i.PartidoId == partidoId)
            .Sum(i => i.CantidadEntradas ?? 0);
    }

    public void Agregar(Inscripcion inscripcion)
    {
        _context.Inscripciones.Add(inscripcion);
        _context.SaveChanges();
    }

    public void Eliminar(int id)
    {
        var inscripcion = _context.Inscripciones.Find(id);
        if (inscripcion is null) return;

        _context.Inscripciones.Remove(inscripcion);
        _context.SaveChanges();
    }
}
