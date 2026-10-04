using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;
using Audit.EntityFramework;

namespace infinitoBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Destino> Destinos { get; set; }

        public DbSet<Excursion> Excursiones { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<PlantillaVehiculo> PlantillasVehiculos { get; set; }

        public DbSet<AsignacionAsiento> AsignacionesAsientos { get; set; }

        public DbSet<ReservaCliente> ReservaCliente { get; set; }

        public DbSet<Asiento> Asientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Excursion>()
                .HasOne(excursion => excursion.Destino)
                .WithMany(destino => destino.Excursiones)
                .HasForeignKey(excursion => excursion.DestinoId);

            modelBuilder.Entity<Excursion>()
                .HasOne(e => e.PlantillaVehiculo)
                .WithMany(pv => pv.Excursiones)
                .HasForeignKey(e => e.PlantillaVehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Destino>()
                .HasMany(destino => destino.Paquetes)
                .WithOne(pais => pais.Destino)
                .HasForeignKey(pais => pais.IdDestino);

            modelBuilder.Entity<Destino>()
                .HasOne(destino => destino.Pais)
                .WithMany(pais => pais.Destinos)
                .HasForeignKey(destino => destino.IdPais);

            modelBuilder.Entity<Reserva>()
    .HasOne(reserva => reserva.ClientePagador)
    .WithMany(cliente => cliente.ReservasPagas)
    .HasForeignKey(reserva => reserva.IdClientePagador)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(reserva => reserva.Excursion)
                .WithMany(excursion => excursion.Reservas)
                .HasForeignKey(reserva => reserva.IdExcursion);

            modelBuilder.Entity<Reserva>()
                .HasOne(reserva => reserva.Paquete)
                .WithMany()
                .HasForeignKey(reserva => reserva.IdPaquete)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasMany(reserva => reserva.ClientesIncluidos)
                .WithMany(cliente => cliente.ReservasIncluido)
                .UsingEntity<ReservaCliente>(
                    l => l.HasOne(rc => rc.Cliente)
                          .WithMany(cliente => cliente.ReservaClientes)
                          .HasForeignKey(rc => rc.ClienteId)
                          .OnDelete(DeleteBehavior.Restrict),

                    r => r.HasOne(rc => rc.Reserva)
                          .WithMany(reserva => reserva.ReservaClientes)
                          .HasForeignKey(rc => rc.ReservaId)
                          .OnDelete(DeleteBehavior.Cascade),

                    j =>
                    {
                        j.HasIndex(rc => new { rc.ReservaId, rc.ClienteId });
                    });

            modelBuilder.Entity<Transaccion>()
                .HasOne(transaccion => transaccion.Cliente)
                .WithMany(cliente => cliente.Transacciones)
                .HasForeignKey(transaccion => transaccion.IdCliente)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaccion>()
                .HasOne(transaccion => transaccion.Reserva)
                .WithMany(reserva => reserva.Transacciones)
                .HasForeignKey(transaccion => transaccion.IdReserva)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReservaCliente>()
                .HasOne(rc => rc.AsignacionAsiento)
                .WithOne(ac => ac.ReservaCliente)
                .HasForeignKey<AsignacionAsiento>(aa => aa.ReservaClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Asiento>()
                .HasOne(a => a.PlantillaVehiculo)
                .WithMany(pv => pv.Asientos)
                .HasForeignKey(a => a.PlantillaVehiculoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AsignacionAsiento>()
                .HasIndex(a => new { a.ExcursionId, a.AsientoId }).IsUnique();

            modelBuilder.Entity<AsignacionAsiento>()
                .HasIndex(a => a.ReservaClienteId).IsUnique();

            modelBuilder.Entity<AsignacionAsiento>()
    .HasOne(aa => aa.Excursion)
    .WithMany(excursion => excursion.Asignaciones)
    .HasForeignKey(aa => aa.ExcursionId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AsignacionAsiento>()
                .HasOne(aa => aa.Asiento)
                .WithMany()
                .HasForeignKey(aa => aa.AsientoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Asiento>()
                .HasIndex(a => new { a.PlantillaVehiculoId, a.PisoAsiento, a.Fila, a.Columna }).IsUnique();
        }
    }
}
