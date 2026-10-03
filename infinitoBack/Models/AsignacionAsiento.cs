namespace infinitoBack.Models
{
    public class AsignacionAsiento
    {
        public int Id { get; set; }

        public int ExcursionId { get; set; }

        public Excursion Excursion { get; set; } = null!;

        public int AsientoId { get; set; }

        public Asiento Asiento { get; set; } = null!;

        public int ReservaClienteId { get; set; }

        public ReservaCliente ReservaCliente { get; set; } = null!;

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    }
}
