using Microsoft.EntityFrameworkCore;
using infinitoBack.Models;

namespace infinitoBack.Data
{
    public class AuditDbContext : DbContext
    {
        public static string? CadenaConexion { get; set; }

        public AuditDbContext()
        {
        }

        public AuditDbContext(DbContextOptions<AuditDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured &&
                !string.IsNullOrEmpty(CadenaConexion))
            {
                optionsBuilder.UseSqlServer(CadenaConexion);
            }
        }
    }
}