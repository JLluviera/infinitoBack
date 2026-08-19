namespace infinitoBack.Models
{
    public class Pais
    {
        public int Id { get; set; }

        public string? NombrePais { get; set; }

        public string? CodigoPais { get; set; }

        public List<Destino>? Destinos { get; set; }
    }
}
