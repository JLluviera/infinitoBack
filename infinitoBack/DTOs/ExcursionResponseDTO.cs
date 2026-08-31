using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class ExcursionResponseDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateOnly FechaSalida { get; set; } 
        public int CantDias { get; set; }

        public int CantLugares { get; set; }
        public string? Descripcion { get; set; }
        public int DestinoId { get; set; }
    }
}
