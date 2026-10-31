using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos;

/// <summary>
/// Datos de ejemplo que se insertan al crear la base.
/// </summary>
internal static class DatosIniciales
{
    public static void Cargar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Partido>().HasData(
            new Partido { Id = 1, Rival = "Brasil", Fecha = new DateTime(2027, 3, 25, 21, 0, 0), Estadio = "Estadio Monumental", Competicion = "Eliminatorias Sudamericanas", CupoEntradas = 500 },
            new Partido { Id = 2, Rival = "Uruguay", Fecha = new DateTime(2027, 3, 30, 20, 30, 0), Estadio = "Estadio Mario A. Kempes", Competicion = "Eliminatorias Sudamericanas", CupoEntradas = 300 },
            new Partido { Id = 3, Rival = "Chile", Fecha = new DateTime(2027, 6, 8, 21, 0, 0), Estadio = "Estadio Único Madre de Ciudades", Competicion = "Amistoso internacional", CupoEntradas = 200 },
            new Partido { Id = 4, Rival = "Colombia", Fecha = new DateTime(2027, 9, 9, 21, 0, 0), Estadio = "La Bombonera", Competicion = "Eliminatorias Sudamericanas", CupoEntradas = 250 },
            new Partido { Id = 5, Rival = "Francia", Fecha = new DateTime(2027, 11, 16, 17, 0, 0), Estadio = "Estadio Monumental", Competicion = "Amistoso internacional", CupoEntradas = 400 }
        );

        modelBuilder.Entity<Socio>().HasData(
            new Socio { Id = 1, NumeroSocio = 10, Nombre = "Lucía", Apellido = "Fernández", Dni = "35123456", FechaNacimiento = new DateTime(1990, 6, 24), Email = "lucia.fernandez@mail.com", Telefono = "11 4567-8910", Domicilio = "Av. Rivadavia 1234", Provincia = "CABA", Categoria = CategoriaSocio.Activo, FechaAlta = new DateTime(2015, 3, 1) },
            new Socio { Id = 2, NumeroSocio = 22, Nombre = "Martín", Apellido = "Gómez", Dni = "28987654", FechaNacimiento = new DateTime(1981, 1, 15), Email = "martin.gomez@mail.com", Telefono = "351 555-1234", Domicilio = "Bv. San Juan 450", Provincia = "Córdoba", Categoria = CategoriaSocio.Vitalicio, FechaAlta = new DateTime(2001, 8, 10) },
            new Socio { Id = 3, NumeroSocio = 105, Nombre = "Valentina", Apellido = "Ruiz", Dni = "50111222", FechaNacimiento = new DateTime(2011, 4, 2), Email = "vale.ruiz@mail.com", Telefono = "11 2233-4455", Domicilio = "Calle 7 N° 820", Provincia = "Buenos Aires", Categoria = CategoriaSocio.Menor, FechaAlta = new DateTime(2023, 1, 20) },
            new Socio { Id = 4, NumeroSocio = 333, Nombre = "Diego", Apellido = "Sosa", Dni = "40555666", FechaNacimiento = new DateTime(1997, 11, 30), Email = "diego.sosa@mail.com", Telefono = "341 444-7788", Domicilio = "Pellegrini 2100", Provincia = "Santa Fe", Categoria = CategoriaSocio.Adherente, FechaAlta = new DateTime(2020, 12, 18) },
            new Socio { Id = 5, NumeroSocio = 1, Nombre = "Juan", Apellido = "Pérez", Dni = "38000000", FechaNacimiento = new DateTime(1995, 1, 1), Email = "juan.perez@mail.com", Telefono = "11 5000-0000", Domicilio = "Florencio Varela 1903", Provincia = "Buenos Aires", Categoria = CategoriaSocio.Activo, FechaAlta = new DateTime(2024, 1, 1) }
        );

        modelBuilder.Entity<Inscripcion>().HasData(
            new Inscripcion { Id = 1, SocioId = 1, PartidoId = 1, Sector = SectorEstadio.PlateaBaja, CantidadEntradas = 2, FechaInscripcion = new DateTime(2026, 10, 1, 10, 30, 0) },
            new Inscripcion { Id = 2, SocioId = 2, PartidoId = 2, Sector = SectorEstadio.Preferencial, CantidadEntradas = 1, FechaInscripcion = new DateTime(2026, 10, 5, 18, 0, 0) }
        );
    }
}
