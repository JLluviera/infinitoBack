using infinitoBack.Enum;

namespace infinitoBack.Models
{
    public class Transaccion
    {
        public int Id { get; set; } 
        public decimal Monto { get; set; }
        public DateOnly FechaCreacion { get; set; }
        public FormaDePago FormaDePago { get; set; }
        public string Observaciones { get; set; }
        public EstadoTransaccion Estado {  get; set; }

        public int IdReserva {  get; set; }
        public Reserva Reserva { get; set; }
        public int IdCliente { get; set; }  
        public Cliente Cliente { get; set; }
    }
}
