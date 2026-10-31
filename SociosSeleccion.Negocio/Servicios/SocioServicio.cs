using SociosSeleccion.Datos.Repositorios;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Negocio.Servicios;

public class SocioServicio : ISocioServicio
{
    private const int EdadMayoria = 18;

    private readonly ISocioRepositorio _socioRepositorio;

    public SocioServicio(ISocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    public List<Socio> Buscar(SocioFiltro filtro)
    {
        return _socioRepositorio.Buscar(filtro);
    }

    public List<Socio> ObtenerTodos()
    {
        return _socioRepositorio.ObtenerTodos();
    }

    public Socio? ObtenerPorId(int id)
    {
        return _socioRepositorio.ObtenerPorId(id);
    }

    public void Crear(Socio socio)
    {
        Normalizar(socio);
        Validar(socio);
        _socioRepositorio.Agregar(socio);
    }

    public void Modificar(Socio socio)
    {
        if (_socioRepositorio.ObtenerPorId(socio.Id) is null)
            throw new NegocioException("El socio que intenta modificar no existe.");

        Normalizar(socio);
        Validar(socio);
        _socioRepositorio.Actualizar(socio);
    }

    public void Eliminar(int id)
    {
        if (_socioRepositorio.ObtenerPorId(id) is null)
            throw new NegocioException("El socio que intenta eliminar no existe.");

        _socioRepositorio.Eliminar(id);
    }

    private static void Normalizar(Socio socio)
    {
        socio.Nombre = socio.Nombre.Trim();
        socio.Apellido = socio.Apellido.Trim();
        socio.Dni = socio.Dni.Trim();
        socio.Email = socio.Email.Trim().ToLower();
        socio.Telefono = socio.Telefono.Trim();
        socio.Domicilio = socio.Domicilio.Trim();
    }

    private void Validar(Socio socio)
    {
        if (!Provincias.Todas.Contains(socio.Provincia))
            throw new NegocioException("La provincia seleccionada no es válida.", nameof(Socio.Provincia));

        if (_socioRepositorio.ExisteNumeroSocio(socio.NumeroSocio!.Value, socio.Id))
            throw new NegocioException("Ya existe otro socio con ese número.", nameof(Socio.NumeroSocio));

        if (_socioRepositorio.ExisteDni(socio.Dni, socio.Id))
            throw new NegocioException("Ya existe otro socio con ese DNI.", nameof(Socio.Dni));

        if (socio.FechaAlta < socio.FechaNacimiento)
            throw new NegocioException("La fecha de alta no puede ser anterior a la fecha de nacimiento.", nameof(Socio.FechaAlta));

        var esMenor = socio.Edad < EdadMayoria;

        if (socio.Categoria == CategoriaSocio.Menor && !esMenor)
            throw new NegocioException($"La categoría Menor es solo para socios de menos de {EdadMayoria} años.", nameof(Socio.Categoria));

        if (socio.Categoria != CategoriaSocio.Menor && esMenor)
            throw new NegocioException($"Los socios de menos de {EdadMayoria} años deben tener categoría Menor.", nameof(Socio.Categoria));
    }
}
