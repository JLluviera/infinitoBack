namespace infinitoBack.ParamsDTOs
{
    public class TransaccionesParamsDTO
    {
        public int? IdCliente { get; set; } 

        public int? IdReserva { get; set; }

        public string? NombreCliente { get; set; }

        public string? ApellidoCliente { get; set; }
        public string? BusquedaCliente { get; set; }
    }
}
