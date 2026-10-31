using Microsoft.Extensions.DependencyInjection;
using SociosSeleccion.Datos;
using SociosSeleccion.Negocio.Servicios;

namespace SociosSeleccion.Negocio;

public static class ConfiguracionNegocio
{
    /// <summary>
    /// Registra los servicios de negocio (y, por debajo, la capa de datos).
    /// </summary>
    public static IServiceCollection AddCapaNegocio(this IServiceCollection services, string cadenaConexion)
    {
        services.AddCapaDatos(cadenaConexion);

        services.AddScoped<ISocioServicio, SocioServicio>();
        services.AddScoped<IPartidoServicio, PartidoServicio>();
        services.AddScoped<IInscripcionServicio, InscripcionServicio>();
        services.AddScoped<IUsuarioServicio, UsuarioServicio>();

        return services;
    }

    /// <summary>
    /// Crea la base de datos y los usuarios iniciales.
    /// </summary>
    public static void InicializarSistema(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        ConfiguracionDatos.CrearBaseDeDatos(scope.ServiceProvider);
        scope.ServiceProvider.GetRequiredService<IUsuarioServicio>().CrearUsuariosIniciales();
    }
}
