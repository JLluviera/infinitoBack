using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class AsientoDTO
    {
        public int Id { get; set; }
        public string NumeroAsiento { get; set; }
        public int PisoAsiento { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoAsiento TipoAsiento { get; set; }
    }
}
