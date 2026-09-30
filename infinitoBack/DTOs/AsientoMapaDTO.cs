using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class AsientoMapaDTO
    {
        public int Id { get; set; }
        public string NumeroAsiento { get; set; }
        public int PisoAsiento { get; set; } 
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoAsiento TipoAsiento { get; set; }
        public bool Ocupado { get; set; }
        public int? ReservaClienteId { get; set; }
        public string? NombreCliente { get; set; }
    }
}
