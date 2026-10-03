using infinitoBack.Models;

namespace infinitoBack.ResponseDTOs
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int? Ci { get; set; }

        public DateOnly? FechaVencimientoCi { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? Telefono { get; set; }

        public List<ReservaResponseDTO>? Reservas { get; set; }

        public List<ReservaResponseDTO>? ReservasPagas { get; set; }
    }
}
