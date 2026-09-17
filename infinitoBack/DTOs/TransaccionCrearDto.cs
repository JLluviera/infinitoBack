using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class TransaccionCrearDto
    {
        public decimal Monto { get; set; }
        public DateOnly FechaCreacion { get; set; }
        public FormaDePago FormaDePago { get; set; }
        public string Observaciones { get; set; }
        public EstadoTransaccion Estado {  get; set; }
        public int IdReserva {  get; set; }
        public int IdCliente { get; set; }


    }
}
