using Microsoft.EntityFrameworkCore.Storage;
using infinitoBack.Enum;
using Microsoft.Identity.Client;

namespace infinitoBack.Models
{
    public class Excursion
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public DateOnly FechaSalida { get; set; }
        public int DuracionDias {  get; set; }
        public int CantLugares { get; set; }
        public string? Descripcion { get; set; }
        public int DestinoId { get; set; }
        public Destino? Destino { get; set; }
        public List<Paquete>? Paquetes { get; set; }
    }
}
