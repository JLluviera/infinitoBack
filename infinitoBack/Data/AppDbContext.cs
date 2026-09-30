using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;

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

        public DbSet<PlantillaVehiculo> PlantillasVehiculos { get; set; }

        public DbSet<AsignacionAsiento> AsignacionesAsientos { get; set; }

        public DbSet<ReservaCliente> ReservaCliente { get; set; }

        public DbSet<Asiento> Asientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Excursion>()
                .HasOne(e => e.Destino)
                .WithMany(d => d.Excursiones)
                .HasForeignKey(e => e.DestinoId);

            modelBuilder.Entity<Excursion>()
                .HasOne(e => e.PlantillaVehiculo)
                .WithMany(pv => pv.Excursiones)
                .HasForeignKey(e => e.PlantillaVehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Destino>()
                .HasMany(d => d.Paquetes)
                .WithOne(p => p.Destino)
                .HasForeignKey(p => p.IdDestino);

            modelBuilder.Entity<Destino>()
                .HasOne(d => d.Pais)
                .WithMany(p => p.Destinos)
                .HasForeignKey(d => d.IdPais);   

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.ClientePagador)
                .WithMany(c => c.ReservasPagas)
                .HasForeignKey(r => r.IdClientePagador)
                .OnDelete(DeleteBehavior.Restrict);
               
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Excursion)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.IdExcursion);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Paquete)
                .WithMany()
                .HasForeignKey(r => r.IdPaquete)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasMany(r => r.ClientesIncluidos)
                .WithMany(c => c.ReservasIncluido)
                .UsingEntity<ReservaCliente>(
                    // Configuración lado Cliente
                    l => l.HasOne(rc => rc.Cliente)
                          .WithMany(c => c.ReservaClientes)
                          .HasForeignKey(rc => rc.ClienteId)
                          .OnDelete(DeleteBehavior.Restrict), // Bloquea si el cliente está asignado a un asiento

                    // Configuración lado Reserva
                    r => r.HasOne(rc => rc.Reserva)
                          .WithMany(res => res.ReservaClientes)
                          .HasForeignKey(rc => rc.ReservaId)
                          .OnDelete(DeleteBehavior.Cascade), // Si se borra la reserva, limpia los asientos automáticamente

                    // Configuración de la tabla intermedia propiamente dicha
                    j =>
                    {
                        j.HasIndex(rc => new { rc.ReservaId, rc.ClienteId });
                    });

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Cliente)
                .WithMany(c => c.Transacciones)
                .HasForeignKey(t => t.IdCliente)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Reserva)
                .WithMany(r => r.Transacciones)
                .HasForeignKey(t => t.IdReserva)
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
                .WithMany()
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
