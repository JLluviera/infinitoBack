using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class ExcursionCrearDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public DateOnly FechaSalida { get; set; } 
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; }
        public TipoPaquete Tipo { get; set; }
        public int DestinoId { get; set; }
    }
}
