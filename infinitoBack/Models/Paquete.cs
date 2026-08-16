using Microsoft.EntityFrameworkCore.Storage;
using infinitoBack.Enum;
using Microsoft.Identity.Client;

namespace infinitoBack.Models
{
    public class Paquete
    {
        public int Id { get; set; }

        public int IdExcursion { get; set; }    
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Seña { get; set; }
        public string Descripcion { get; set; }
        public Excursion Excursion { get; set; }

    }
}
