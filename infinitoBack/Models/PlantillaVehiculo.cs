namespace infinitoBack.Models
{
    public class PlantillaVehiculo
    {
        public int Id { get; set; }

        public string NombrePlantilla { get; set; } = string.Empty;

        public int TotalPisos { get; set; }

        public int TotalFilas { get; set; }

        public int TotalColumnas { get; set; }

        public List<Asiento> Asientos { get; set; } = new List<Asiento>();

        public List<Excursion> Excursiones { get; set; } = new List<Excursion>();

    }
}
