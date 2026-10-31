using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SociosSeleccion.Datos.Repositorios;

namespace SociosSeleccion.Datos;

public static class ConfiguracionDatos
{
    /// <summary>
    /// Registra el DbContext y los repositorios de la capa de datos.
    /// </summary>
    public static IServiceCollection AddCapaDatos(this IServiceCollection services, string cadenaConexion)
    {
        services.AddDbContext<AppDbContext>(o => o.UseSqlite(cadenaConexion));

        services.AddScoped<ISocioRepositorio, SocioRepositorio>();
        services.AddScoped<IPartidoRepositorio, PartidoRepositorio>();
        services.AddScoped<IInscripcionRepositorio, InscripcionRepositorio>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

        return services;
    }

    /// <summary>
    /// Crea la base de datos (con los datos de ejemplo) si todavía no existe.
    /// </summary>
    public static void CrearBaseDeDatos(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
}
