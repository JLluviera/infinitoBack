using infinitoBack.Enum;

namespace infinitoBack.DTOs
{
    public class CrearPlantillaVehiculoDTO
    {
        public string NombrePlantilla { get; set; } = string.Empty;
        public int TotalPisos { get; set; }

        public int TotalFilas { get; set; }

        public int TotalColumnas { get; set; }
        public List<CrearAsientoDTO> Asientos { get; set; } = new List<CrearAsientoDTO>();
    }
}
