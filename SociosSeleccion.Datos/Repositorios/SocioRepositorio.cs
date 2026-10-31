using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos.Repositorios;

public class SocioRepositorio : ISocioRepositorio
{
    private readonly AppDbContext _context;

    public SocioRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public List<Socio> Buscar(SocioFiltro filtro)
    {
        var consulta = _context.Socios
            .Include(s => s.Inscripciones)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Dni))
        {
            var dni = filtro.Dni.Trim();
            consulta = consulta.Where(s => s.Dni.Contains(dni));
        }

        if (filtro.Categoria.HasValue)
        {
            consulta = consulta.Where(s => s.Categoria == filtro.Categoria);
        }

        var socios = consulta
            .OrderBy(s => s.Apellido)
            .ThenBy(s => s.Nombre)
            .ToList();

        // El nombre se filtra en memoria para ignorar mayúsculas y tildes ("gomez" encuentra a "Gómez")
        if (!string.IsNullOrWhiteSpace(filtro.Apellido))
        {
            var texto = SinTildes(filtro.Apellido.Trim());
            socios = socios
                .Where(s => SinTildes(s.Apellido).Contains(texto) || SinTildes(s.Nombre).Contains(texto))
                .ToList();
        }

        return socios;
    }

    private static string SinTildes(string texto)
    {
        var descompuesto = texto.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sinMarcas = descompuesto.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        return new string(sinMarcas.ToArray());
    }

    public List<Socio> ObtenerTodos()
    {
        return _context.Socios
            .AsNoTracking()
            .OrderBy(s => s.Apellido)
            .ThenBy(s => s.Nombre)
            .ToList();
    }

    public Socio? ObtenerPorId(int id)
    {
        return _context.Socios
            .Include(s => s.Inscripciones)
                .ThenInclude(i => i.Partido)
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == id);
    }

    public bool ExisteNumeroSocio(int numeroSocio, int idExcluido)
    {
        return _context.Socios.Any(s => s.NumeroSocio == numeroSocio && s.Id != idExcluido);
    }

    public bool ExisteDni(string dni, int idExcluido)
    {
        return _context.Socios.Any(s => s.Dni == dni && s.Id != idExcluido);
    }

    public void Agregar(Socio socio)
    {
        _context.Socios.Add(socio);
        _context.SaveChanges();
    }

    public void Actualizar(Socio socio)
    {
        var existente = _context.Socios.Find(socio.Id)
            ?? throw new InvalidOperationException($"No existe el socio con Id {socio.Id}.");

        // Copia solo los campos simples (no toca las inscripciones)
        _context.Entry(existente).CurrentValues.SetValues(socio);
        _context.SaveChanges();
    }

    public void Eliminar(int id)
    {
        var socio = _context.Socios.Find(id);
        if (socio is null) return;

        _context.Socios.Remove(socio);
        _context.SaveChanges();
    }
}
