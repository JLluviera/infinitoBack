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
        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<Excursion> Excursiones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Excursion>()
                .HasOne(e => e.Destino)
                .WithMany(d => d.Excursiones)
                .HasForeignKey(e => e.DestinoId);

        }
    }
}
