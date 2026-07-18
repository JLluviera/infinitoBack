using Microsoft.EntityFrameworkCore.Storage;

namespace infinitoBack.Models
{
    public class Paquete
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin {  get; set; }
        
        public int DestinoId { get; set; }
        public Destino Destino { get; set; }


    }
}
