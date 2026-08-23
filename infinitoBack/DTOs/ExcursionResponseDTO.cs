using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class ExcursionResponseDTO
    {
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public DateOnly FechaSalida { get; set; } 
        public int CantDias { get; set; }

        public int CantLugares { get; set; }
        public string? Descripcion { get; set; }
        public int DestinoId { get; set; }
    }
}
