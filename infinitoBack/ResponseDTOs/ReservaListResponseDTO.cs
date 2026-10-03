using infinitoBack.Enum;

namespace infinitoBack.ResponseDTOs
{
    public class ReservaListResponseDTO
    {
        public int Id { get; set; }
        
        public int IdExcursion { get; set; }
        
        public string NombreCliente { get; set; }

        public string ApellidoCliente { get; set; }

        public int CiCliente { get; set; }

        public EstadoRes EstadoReserva { get; set; }
    }
}
