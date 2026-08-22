using infinitoBack.Models;

namespace infinitoBack.DTOs
{
    public class ExcursionResponseDTO
    {
        public string Nombre { get; set; }

        public int CantLugares { get; set; }

        public int CantDias { get; set; }

        public DateOnly FechaSalida { get; set; }

        public int DestinoId { get; set; }
    }
}
