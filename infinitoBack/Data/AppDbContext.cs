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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Excursion>()
                .HasOne(excursion => excursion.Destino)
                .WithMany(destino => destino.Excursiones)
                .HasForeignKey(excursion => excursion.DestinoId);

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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reserva>()
                .HasMany(reserva => reserva.ClientesIncluidos)
                .WithMany(cliente => cliente.Reservas)
                .UsingEntity<Dictionary<string, object>>(
                    "ReservaCliente",
                    j => j.HasOne<Cliente>().WithMany().HasForeignKey("ClienteId"),
                    j => j.HasOne<Reserva>().WithMany().HasForeignKey("ReservaId"));
                
            modelBuilder.Entity<Reserva>()
                .HasOne(reserva => reserva.Excursion)
                .WithMany(excursion => excursion.Reservas)
                .HasForeignKey(reserva => reserva.IdExcursion);

            modelBuilder.Entity<Reserva>()
                .HasOne(reserva => reserva.Paquete)
                .WithMany()
                .HasForeignKey(reserva => reserva.IdPaquete)
                .OnDelete(DeleteBehavior.Restrict);

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
        }
    }
}
