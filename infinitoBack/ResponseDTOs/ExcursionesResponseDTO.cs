using infinitoBack.Models;
using infinitoBack.DTOs;
using infinitoBack.ResponseDTOs;

namespace infinitoBack.ResponseDTOs
{
    public class ExcursionesResponseDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CantLugares { get; set; }

        public int CantDias { get; set; }

        public DateOnly FechaSalida { get; set; }

        public DestinoCrearDto Destino { get; set; }

        public List<PaqueteResponseDTO> Paquetes { get; set; }
    }
}
