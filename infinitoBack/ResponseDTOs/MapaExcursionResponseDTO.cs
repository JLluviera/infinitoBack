using infinitoBack.DTOs;

namespace infinitoBack.ResponseDTOs
{
    public class MapaExcursionResponseDTO
    {
        public int ExcursionId { get; set; }
        public string NombreExcursion { get; set; } = string.Empty;
        public int PlantillaVehiculoId { get; set; }
        public string NombrePlantilla { get; set; } = string.Empty;
        public int TotalPisos { get; set; }
        public int TotalFilas { get; set; }
        public int TotalColumnas { get; set; }
        public List<AsientoMapaDTO> Asientos { get; set; } = new List<AsientoMapaDTO>();
        public List<PasajeroPendienteDTO> PasajerosPendientes { get; set; } = new List<PasajeroPendienteDTO>();
    }
}
