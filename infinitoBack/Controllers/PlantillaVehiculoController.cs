using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Models;
using infinitoBack.ResponseDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantillaVehiculoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlantillaVehiculoController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlantillas()
        {
            List<PlantillaVehiculoDTO> plantillas = await _context.PlantillasVehiculos
                .Include(p => p.Asientos)
                .Select(p => new PlantillaVehiculoDTO()
                {
                    Id = p.Id,
                    NombrePlantilla = p.NombrePlantilla,
                    TotalPisos = p.TotalPisos,
                    TotalFilas = p.TotalFilas,
                    TotalColumnas = p.TotalColumnas,
                    Asientos = p.Asientos.Select(a => new AsientoDTO() {
                        Id = a.Id,
                        NumeroAsiento = a.NumeroAsiento,
                        PisoAsiento = a.PisoAsiento,
                        Fila = a.Fila,
                        Columna = a.Columna,
                        TipoAsiento = a.TipoAsiento
                    }).ToList()
                })
                .ToListAsync();

            return Ok(plantillas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlantillaVehiculoDTO>> GetPlantilla(int id)
        {
            var plantilla = await _context.PlantillasVehiculos
                .Include(p => p.Asientos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plantilla == null) return NotFound("Plantilla no encontrada.");

            PlantillaVehiculoDTO dto = new PlantillaVehiculoDTO() {
                Id = plantilla.Id,
                NombrePlantilla = plantilla.NombrePlantilla,
                TotalPisos = plantilla.TotalPisos,
                TotalColumnas = plantilla.TotalColumnas,
                TotalFilas = plantilla.TotalFilas,
                Asientos = plantilla.Asientos.Select(a => new AsientoDTO() {
                    Id = a.Id,
                    NumeroAsiento = a.NumeroAsiento,
                    PisoAsiento = a.PisoAsiento,
                    Fila = a.Fila,
                    Columna = a.Columna,
                    TipoAsiento = a.TipoAsiento
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPlantilla([FromBody] CrearPlantillaVehiculoDTO dto)
        {
            PlantillaVehiculo nuevaPlantilla = new PlantillaVehiculo
            {
                NombrePlantilla = dto.NombrePlantilla,
                TotalPisos = dto.TotalPisos,
                TotalColumnas = dto.TotalColumnas,
                TotalFilas = dto.TotalFilas,
                Asientos = dto.Asientos.Select(a => new Asiento
                {
                    NumeroAsiento = a.NumeroAsiento,
                    PisoAsiento = a.PisoAsiento,
                    Fila = a.Fila,
                    Columna = a.Columna,
                    TipoAsiento = a.TipoAsiento
                }).ToList()
            };

            _context.PlantillasVehiculos.Add(nuevaPlantilla);
            await _context.SaveChangesAsync();

            return Ok("Plantilla creada");
        }
    }
}
