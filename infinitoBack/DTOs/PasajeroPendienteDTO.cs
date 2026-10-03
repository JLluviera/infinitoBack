namespace infinitoBack.DTOs
{
    public class PasajeroPendienteDTO
    {
        public int ReservaClienteId { get; set; }
        public int ReservaId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
    }
}
