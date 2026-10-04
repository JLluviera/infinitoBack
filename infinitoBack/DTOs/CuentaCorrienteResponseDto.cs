namespace infinitoBack.DTOs
{
    public class CuentaCorrienteResponseDTO
    {
        public int IdReserva { get; set; }
        public int IdExcursion { get; set; }
        public string NombreExcursion { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}
