namespace infinitoBack.Models
{
    public class Destino
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public int IdPais { get; set; }
        public string Descripcion { get; set; }

        public Pais Pais { get; set; }
        public List<Excursion> Excursiones { get; set; }
    }
}
