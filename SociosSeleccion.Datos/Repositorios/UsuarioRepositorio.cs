using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AppDbContext _context;

    public UsuarioRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public List<Usuario> ObtenerTodos()
    {
        return _context.Usuarios
            .Include(u => u.Socio)
                .ThenInclude(s => s!.Inscripciones)
                    .ThenInclude(i => i.Partido)
            .AsNoTracking()
            .OrderBy(u => u.Rol)
            .ThenBy(u => u.NombreUsuario)
            .ToList();
    }

    public Usuario? ObtenerPorNombreUsuario(string nombreUsuario)
    {
        return _context.Usuarios.AsNoTracking().FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
    }

    public bool ExisteParaSocio(int socioId)
    {
        return _context.Usuarios.Any(u => u.SocioId == socioId);
    }

    public bool HayUsuarios()
    {
        return _context.Usuarios.Any();
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }
}
