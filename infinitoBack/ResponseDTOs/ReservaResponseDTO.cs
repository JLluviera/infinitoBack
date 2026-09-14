using infinitoBack.Enum;

namespace infinitoBack.ResponseDTOs
{
    public class ReservaResponseDTO
    {
        public int Id { get; set; }

        public int IdClientePagador { get; set; }

        public int IdExcursion { get; set; }

        public int IdPaquete { get; set; }

        public decimal MontoTotal { get; set; }

        public EstadoRes EstadoReserva { get; set; }

        public DateTime FechaReserva { get; set; }

        public ClienteResponseDTO? ClientePagador { get; set; }

        public ExcursionesResponseDTO? Excursion { get; set; }

        public PaqueteResponseDTO? Paquete { get; set; }

        public List<ClienteResponseDTO>? ClientesIncluidos { get; set; }
    }
}
