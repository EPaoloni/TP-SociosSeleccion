using SociosSeleccion.Entidades;

namespace SociosSeleccion.Web.Models;

public class PartidoResumen
{
    public Partido Partido { get; set; } = new();
    public List<Inscripcion> Inscripciones { get; set; } = new();
    public int EntradasDisponibles { get; set; }

    public int EntradasReservadas => (Partido.CupoEntradas ?? 0) - EntradasDisponibles;
    public bool Jugado => Partido.Fecha < DateTime.Now;
    public bool Agotado => EntradasDisponibles <= 0;
}
