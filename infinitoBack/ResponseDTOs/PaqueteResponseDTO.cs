using infinitoBack.Models;

namespace infinitoBack.ResponseDTOs
{
    public class PaqueteResponseDTO
    {
        public int Id { get; set; }
        public int IdDestino { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public string Descripcion { get; set; }

        public Destino? Destino { get; set; }
    }
}
