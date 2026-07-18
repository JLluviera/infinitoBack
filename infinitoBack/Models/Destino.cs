namespace infinitoBack.Models
{
    public class Destino
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Descripcion { get; set; }
        public List<Paquete> Paquetes { get; set; }
    }
}
