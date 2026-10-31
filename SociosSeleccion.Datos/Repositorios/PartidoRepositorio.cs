using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos.Repositorios;

public class PartidoRepositorio : IPartidoRepositorio
{
    private readonly AppDbContext _context;

    public PartidoRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public List<Partido> ObtenerTodos()
    {
        return _context.Partidos
            .AsNoTracking()
            .OrderBy(p => p.Fecha)
            .ToList();
    }

    public List<Partido> ObtenerDesde(DateTime fecha)
    {
        return _context.Partidos
            .AsNoTracking()
            .Where(p => p.Fecha >= fecha)
            .OrderBy(p => p.Fecha)
            .ToList();
    }

    public Partido? ObtenerPorId(int id)
    {
        return _context.Partidos.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public bool ExisteEnElDia(DateTime fecha, int idExcluido)
    {
        var inicio = fecha.Date;
        var fin = inicio.AddDays(1);
        return _context.Partidos.Any(p => p.Fecha >= inicio && p.Fecha < fin && p.Id != idExcluido);
    }

    public void Agregar(Partido partido)
    {
        _context.Partidos.Add(partido);
        _context.SaveChanges();
    }

    public void Actualizar(Partido partido)
    {
        var existente = _context.Partidos.Find(partido.Id)
            ?? throw new InvalidOperationException($"No existe el partido con Id {partido.Id}.");

        _context.Entry(existente).CurrentValues.SetValues(partido);
        _context.SaveChanges();
    }
}
