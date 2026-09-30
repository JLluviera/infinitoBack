using Microsoft.EntityFrameworkCore.Storage;
using infinitoBack.Enum;
using Microsoft.Identity.Client;

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

        public Destino Destino { get; set; } = null!;

        public List<Reserva>? Reservas { get; set; } = new List<Reserva>();

        public int? PlantillaVehiculoId { get; set; }

        public PlantillaVehiculo? PlantillaVehiculo { get; set; } = null!;

        public List<AsignacionAsiento> Asignaciones { get; set; } = new List<AsignacionAsiento>();
    }
}
