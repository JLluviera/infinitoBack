namespace infinitoBack.Models
{
    public class Excursion
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CantLugares { get; set; }

        public int CantDias { get; set; }

        public DateOnly FechaSalida { get; set; }

        public int DestinoId { get; set; }
        public Destino Destino { get; set; }

        public Excursion() { }
    }
}
