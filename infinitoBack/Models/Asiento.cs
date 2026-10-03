using infinitoBack.Enum;

namespace infinitoBack.Models
{
    public class Asiento
    {
        public int Id { get; set; }

        public int PlantillaVehiculoId { get; set; }

        public PlantillaVehiculo PlantillaVehiculo { get; set; } = null!;

        public string NumeroAsiento { get; set; } = string.Empty;

        public int PisoAsiento { get; set; }

        public int Fila { get; set; }

        public int Columna { get; set; }

        public TipoAsiento TipoAsiento { get; set; }
    }
}
