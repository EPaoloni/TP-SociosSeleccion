using Microsoft.EntityFrameworkCore;
using SociosSeleccion.Entidades;

namespace SociosSeleccion.Datos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Socio> Socios => Set<Socio>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Socio>(e =>
        {
            e.HasIndex(s => s.NumeroSocio).IsUnique();
            e.HasIndex(s => s.Dni).IsUnique();
            e.Property(s => s.Nombre).HasMaxLength(50);
            e.Property(s => s.Apellido).HasMaxLength(50);
            e.Property(s => s.Dni).HasMaxLength(8);
            e.Property(s => s.Email).HasMaxLength(100);
            e.Property(s => s.Telefono).HasMaxLength(20);
            e.Property(s => s.Domicilio).HasMaxLength(120);
            e.Property(s => s.Provincia).HasMaxLength(50);
        });

        modelBuilder.Entity<Partido>(e =>
        {
            e.Property(p => p.Rival).IsRequired().HasMaxLength(50);
            e.Property(p => p.Estadio).IsRequired().HasMaxLength(80);
            e.Property(p => p.Competicion).IsRequired().HasMaxLength(80);
        });

        modelBuilder.Entity<Inscripcion>(e =>
        {
            e.HasOne(i => i.Socio)
                .WithMany(s => s.Inscripciones)
                .HasForeignKey(i => i.SocioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(i => i.Partido)
                .WithMany()
                .HasForeignKey(i => i.PartidoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Un socio no puede anotarse dos veces al mismo partido
            e.HasIndex(i => new { i.SocioId, i.PartidoId }).IsUnique();
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasIndex(u => u.NombreUsuario).IsUnique();
            e.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(30);
            e.Property(u => u.NombreCompleto).IsRequired().HasMaxLength(80);

            // Cada socio puede tener un único usuario; si se elimina el socio, se elimina su usuario
            e.HasOne(u => u.Socio)
                .WithOne()
                .HasForeignKey<Usuario>(u => u.SocioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        DatosIniciales.Cargar(modelBuilder);
    }
}
