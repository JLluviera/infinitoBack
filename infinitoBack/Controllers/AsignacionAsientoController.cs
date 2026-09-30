using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Models;
using infinitoBack.ResponseDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AsignacionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asignaciones/{excursionId}/mapa-asientos
        // GET: api/asignaciones/{excursionId}/mapa-asientos
        [HttpGet("{excursionId}/mapa-asientos")]
        public async Task<ActionResult<MapaExcursionResponseDTO>> GetMapaAsientos(int excursionId)
        {
            // Aseguramos incluir correctamente todas las cadenas de navegación necesarias
            Excursion? excursion = await _context.Excursiones
                .Include(e => e.PlantillaVehiculo)
                    .ThenInclude(p => p.Asientos)
                .Include(e => e.Asignaciones)
                    .ThenInclude(a => a.ReservaCliente)
                        .ThenInclude(rc => rc.Cliente)
                .Include(e => e.Reservas!)
                    .ThenInclude(r => r.ReservaClientes) // 🔥 Corregido para que coincida con el SelectMany de abajo
                        .ThenInclude(rc => rc.Cliente)   // 🔥 Cargamos el cliente de la reserva pendiente
                .FirstOrDefaultAsync(e => e.Id == excursionId);

            if (excursion == null) return NotFound("Excursión no encontrada.");

            List<AsientoMapaDTO> asientosMapa = new List<AsientoMapaDTO>();

            // 1. Mapear asientos con estado de ocupación de forma segura
            if (excursion.PlantillaVehiculo != null && excursion.PlantillaVehiculo.Asientos != null)
            {
                asientosMapa = excursion.PlantillaVehiculo.Asientos.Select(asiento =>
                {
                    var asignacion = excursion.Asignaciones?
                        .FirstOrDefault(a => a.AsientoId == asiento.Id);

                    return new AsientoMapaDTO()
                    {
                        Id = asiento.Id,
                        NumeroAsiento = asiento.NumeroAsiento,
                        PisoAsiento = asiento.PisoAsiento,
                        Fila = asiento.Fila,
                        Columna = asiento.Columna,
                        TipoAsiento = asiento.TipoAsiento,
                        Ocupado = asignacion != null,
                        ReservaClienteId = asignacion?.ReservaClienteId,
                        // Appendeamos de forma segura usando navegación segura (?.)
                        NombreCliente = asignacion?.ReservaCliente?.Cliente != null
                            ? $"{asignacion.ReservaCliente.Cliente.Nombre}"
                            : null
                    };
                }).ToList();
            }

            // 2. Mapear pasajeros pendientes de forma segura
            List<PasajeroPendienteDTO> pasajerosPendientes = new List<PasajeroPendienteDTO>();

            if (excursion.Reservas != null && excursion.Asignaciones != null)
            {
                pasajerosPendientes = excursion.Reservas
                    .Where(r => r.ReservaClientes != null)
                    .SelectMany(r => r.ReservaClientes)
                    .Where(rc => rc.Cliente != null && !excursion.Asignaciones.Any(a => a.ReservaClienteId == rc.ClienteId && a.ExcursionId == excursion.Id))
                    .Select(rc => new PasajeroPendienteDTO
                    {
                        ReservaClienteId = rc.ClienteId,
                        ReservaId = rc.ReservaId,
                        ClienteId = rc.ClienteId,
                        NombreCliente = rc.Cliente?.Nombre ?? "Sin Nombre",   // Evita nulos si el objeto cliente fallara
                        ApellidoCliente = rc.Cliente?.Apellido ?? "Sin Apellido"
                    }).ToList();
            }

            // 3. Construcción del resultado blindada contra PlantillaVehiculo = null
            var resultado = new MapaExcursionResponseDTO()
            {
                ExcursionId = excursion.Id,
                NombreExcursion = excursion.Nombre,
                // Usamos ?. e indicamos valores por defecto si el vehículo no está asignado
                PlantillaVehiculoId = excursion.PlantillaVehiculo?.Id ?? 0,
                NombrePlantilla = excursion.PlantillaVehiculo?.NombrePlantilla ?? "Sin Vehículo Asignado",
                TotalPisos = excursion.PlantillaVehiculo?.TotalPisos ?? 0,
                TotalFilas = excursion.PlantillaVehiculo?.TotalFilas ?? 0,
                TotalColumnas = excursion.PlantillaVehiculo?.TotalColumnas ?? 0,
                Asientos = asientosMapa,
                PasajerosPendientes = pasajerosPendientes
            };

            return Ok(resultado);
        }


        // POST: api/asignaciones/asignar-asiento
        [HttpPost("asignar-asiento")]
        public async Task<IActionResult> AsignarAsiento([FromBody] AsignarAsientoRequestDTO dto)
        {
            // Validar que el asiento pertenezca a la plantilla de la excursión
            var excursion = await _context.Excursiones
                .Include(e => e.PlantillaVehiculo)
                .FirstOrDefaultAsync(e => e.Id == dto.ExcursionId);

            if (excursion == null) return NotFound("Excursión no encontrada.");

            bool asientoExisteEnPlantilla = await _context.Asientos
                .AnyAsync(a => a.Id == dto.AsientoId && a.PlantillaVehiculoId == excursion.PlantillaVehiculoId);

            if (!asientoExisteEnPlantilla)
                return BadRequest("El asiento seleccionado no pertenece al vehículo de esta excursión.");

            // Validar si el asiento ya está ocupado en la excursión
            bool asientoOcupado = await _context.AsignacionesAsientos
                .AnyAsync(a => a.ExcursionId == dto.ExcursionId && a.AsientoId == dto.AsientoId && a.ReservaClienteId != dto.ReservaClienteId);

            if (asientoOcupado)
                return BadRequest("El asiento ya se encuentra asignado a otro pasajero.");

            // Buscar si el cliente ya tenía un asiento previo en esta excursión para moverlo
            var asignacionExistente = await _context.AsignacionesAsientos
                .FirstOrDefaultAsync(a => a.ExcursionId == dto.ExcursionId && a.ReservaClienteId == dto.ReservaClienteId);

            if (asignacionExistente != null)
            {
                // Reasignación
                asignacionExistente.AsientoId = dto.AsientoId;
                asignacionExistente.FechaAsignacion = DateTime.UtcNow;
            }
            else
            {
                // Nueva asignación
                var nuevaAsignacion = new AsignacionAsiento
                {
                    ExcursionId = dto.ExcursionId,
                    AsientoId = dto.AsientoId,
                    ReservaClienteId = dto.ReservaClienteId,
                    FechaAsignacion = DateTime.UtcNow
                };

                _context.AsignacionesAsientos.Add(nuevaAsignacion);
            }

            await _context.SaveChangesAsync();
            return Ok("Asiento asignado correctamente.");
        }

        // POST: api/asignaciones/desasignar-asiento
        [HttpPost("desasignar-asiento")]
        public async Task<IActionResult> DesasignarAsiento([FromBody] DesasignarAsientoRequestDTO dto)
        {
            var asignacion = await _context.AsignacionesAsientos
                .FirstOrDefaultAsync(a => a.ExcursionId == dto.ExcursionId && a.AsientoId == dto.AsientoId);

            if (asignacion == null)
                return NotFound("No existe una asignación activa para este asiento.");

            _context.AsignacionesAsientos.Remove(asignacion);
            await _context.SaveChangesAsync();

            return Ok("Asiento liberado correctamente.");
        }
    }
}