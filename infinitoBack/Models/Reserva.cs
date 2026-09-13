using infinitoBack.Enum;
using System.ComponentModel.DataAnnotations;

namespace infinitoBack.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateTime FechaReserva { get; set; } = DateTime.Now;

        public EstadoRes EstadoReserva { get; set; } = EstadoRes.Pendiente;

        public decimal MontoTotal { get; set; }

        public int IdClientePagador { get; set; }

        [Required]
        public Cliente ClientePagador { get; set; }

        [Required]
        public int IdExcursion { get; set; }

        public Excursion Excursion { get; set; }
        [Required]
        public int IdPaquete { get; set; }
        
        public Paquete Paquete { get; set; }

        public List<Cliente>? ClientesIncluidos { get; set; }
    }
}

