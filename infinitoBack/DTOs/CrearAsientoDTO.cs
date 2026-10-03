using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class CrearAsientoDTO
    {
        public string NumeroAsiento { get; set; }
        public int PisoAsiento { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoAsiento TipoAsiento { get; set; }
    }
}
