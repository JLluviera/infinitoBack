using infinitoBack.DTOs;

namespace infinitoBack.DTOs
{
    public class PlantillaVehiculoDTO
    {
        public int Id { get; set; }
        public string NombrePlantilla { get; set; } = string.Empty;

        public int TotalFilas { get; set; }

        public int TotalColumnas { get; set; }
        public int TotalPisos { get; set; }
        public List<AsientoDTO> Asientos { get; set; }
    }
}
