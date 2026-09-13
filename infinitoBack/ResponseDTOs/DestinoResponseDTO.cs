using infinitoBack.Models;

namespace infinitoBack.ResponseDTOs
{
    public class DestinoResponseDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Ciudad { get; set; }

        public string Descripcion { get; set; }

        public int IdPais { get; set; }

        public Pais Pais { get; set; }

        public List<PaqueteResponseDTO>? Paquetes { get; set; }

        public List<ExcursionesResponseDTO>? Excursiones { get; set; }
    }
}
