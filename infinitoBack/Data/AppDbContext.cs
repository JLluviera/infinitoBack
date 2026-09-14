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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Excursion>()
                .HasOne(e => e.Destino)
                .WithMany(d => d.Excursiones)
                .HasForeignKey(e => e.DestinoId);

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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reserva>()
                .HasMany(r => r.ClientesIncluidos)
                .WithMany(c => c.Reservas)
                .UsingEntity<Dictionary<string, object>>(
                    "ReservaCliente",
                    j => j.HasOne<Cliente>().WithMany().HasForeignKey("ClienteId"),
                    j => j.HasOne<Reserva>().WithMany().HasForeignKey("ReservaId"));
                
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Excursion)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.IdExcursion);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Paquete)
                .WithMany()
                .HasForeignKey(r => r.IdPaquete)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
