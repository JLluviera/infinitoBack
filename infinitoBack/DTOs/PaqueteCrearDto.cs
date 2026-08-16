using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class PaqueteCrearDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public string Descripcion { get; set; }
        public int IdExcursion { get; set; }
    }
}
