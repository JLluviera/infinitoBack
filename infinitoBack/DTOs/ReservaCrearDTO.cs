namespace infinitoBack.DTOs
{
    public class ReservaCrearDTO
    {
        public int CiClientePagador { get; set; }

        public int IdExcursion { get; set; }

        public int IdPaquete { get; set; }

        public int MontoTotal { get; set; } = 0;

    }
}
