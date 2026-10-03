using Microsoft.Identity.Client;

namespace infinitoBack.Models
{
    public class ReservaCliente
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }

        public Reserva Reserva { get; set; } = null!;

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public int? AsignacionAsientoId { get; set; }

        public AsignacionAsiento? AsignacionAsiento { get; set; }
    }
}
