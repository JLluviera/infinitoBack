using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Models;
using infinitoBack.ResponseDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data;


namespace infinitoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ReservaController>
        [HttpGet]
        public async Task<IActionResult> ListaReservas()
        {
            if (_context.Reservas == null)
            {
                return NotFound("No se encontraron reservas");
            }

            return Ok(_context.Reservas.Select(r => new ReservaResponseDTO
            {
                Id = r.Id,
                IdClientePagador = r.IdClientePagador,
                IdExcursion = r.IdExcursion,
                IdPaquete = r.IdPaquete,
                MontoTotal = r.MontoTotal,
                EstadoReserva = r.EstadoReserva,
                FechaReserva = r.FechaReserva,
                ClientePagador = r.ClientePagador != null ? new ClienteResponseDTO
                {
                    Id = r.ClientePagador.Id,
                    Nombre = r.ClientePagador.Nombre,
                    Apellido = r.ClientePagador.Apellido,
                    Telefono = r.ClientePagador.Telefono,
                    Ci = r.ClientePagador.Ci,
                    FechaNacimiento = r.ClientePagador.FechaNacimiento
                } : null,
                Paquete = r.Paquete != null ? new PaqueteResponseDTO
                {
                    Id = r.Paquete.Id,
                    Nombre = r.Paquete.Nombre,
                    Precio = r.Paquete.Precio,
                } : null,
                Excursion = r.Excursion != null ? new ExcursionesResponseDTO
                {
                    Id = r.Excursion.Id,
                    Nombre = r.Excursion.Nombre
                } : null,
            }));
        }



        // GET api/<ReservaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Ver(int id)
        {
            if (id == 0) return BadRequest("El id no puede ser 0");

            Reserva? reserva = await _context.Reservas
                                                .Include(r => r.ClientePagador)
                                                .Include(r => r.Paquete)
                                                .Include(r => r.Excursion)
                                                .Include(r => r.ClientesIncluidos)
                                                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound("Reserva no encontrada");
            }

            return Ok(new ReservaResponseDTO
            {
                Id = reserva.Id,
                IdClientePagador = reserva.IdClientePagador,
                IdExcursion = reserva.IdExcursion,
                IdPaquete = reserva.IdPaquete,
                MontoTotal = reserva.MontoTotal,
                EstadoReserva = reserva.EstadoReserva,
                FechaReserva = reserva.FechaReserva,
                ClientePagador = reserva.ClientePagador != null ? new ClienteResponseDTO
                {
                    Id = reserva.ClientePagador.Id,
                    Nombre = reserva.ClientePagador.Nombre,
                    Apellido = reserva.ClientePagador.Apellido,
                    Telefono = reserva.ClientePagador.Telefono,
                    Ci = reserva.ClientePagador.Ci,
                    FechaNacimiento = reserva.ClientePagador.FechaNacimiento
                } : null,
                Paquete = reserva.Paquete != null ? new PaqueteResponseDTO
                {
                    Id = reserva.Paquete.Id,
                    Nombre = reserva.Paquete.Nombre,
                    Precio = reserva.Paquete.Precio,
                    Seña = reserva.Paquete.Seña,
                    Descripcion = reserva.Paquete.Descripcion
                } : null,
                Excursion = reserva.Excursion != null ? new ExcursionesResponseDTO
                {
                    Id = reserva.Excursion.Id,
                    Nombre = reserva.Excursion.Nombre,
                    FechaSalida = reserva.Excursion.FechaSalida,
                } : null,
                ClientesIncluidos = reserva.ClientesIncluidos?.Select(c => new ClienteResponseDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Ci = c.Ci,
                }).ToList() ?? null
            });
        }

        // POST api/<ReservaController>
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaCrearDTO reservaNueva)
        {
            if (reservaNueva == null)
            {
                return BadRequest("Datos de reserva inválidos");
            }

            if (reservaNueva.CiClientePagador == 0 || reservaNueva.IdExcursion == 0 || reservaNueva.IdPaquete == 0)
            {
                return BadRequest("Los campos IdClientePagador, IdExcursion e IdPaquete son obligatorios y no pueden ser 0");
            }

            Excursion? excursion = await _context.Excursiones.FindAsync(reservaNueva.IdExcursion);

            if (excursion == null)
            {
                return NotFound($"No se encontró ninguna excursión con el id {reservaNueva.IdExcursion}");
            }

            Cliente? clientePagador = await _context.Clientes.Where(c => c.Ci == reservaNueva.CiClientePagador).FirstOrDefaultAsync();

            if (clientePagador == null)
            {
                return NotFound($"No se encontró ningún cliente con el id {reservaNueva.CiClientePagador}");
            }

            Paquete? paquete = await _context.Paquetes.FindAsync(reservaNueva.IdPaquete);

            if (paquete == null)
            {
                return NotFound($"No se encontró ningún paquete con el id {reservaNueva.IdPaquete}");
            }

            Reserva reserva = new Reserva
            {
                IdClientePagador = clientePagador.Id,
                IdExcursion = reservaNueva.IdExcursion,
                IdPaquete = reservaNueva.IdPaquete,
                ClientePagador = clientePagador,
                Excursion = excursion,
                Paquete = paquete,
                MontoTotal = paquete.Precio,
                ClientesIncluidos = new List<Cliente> { clientePagador }
            };

            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();

            return Ok("Reserva creada exitosamente");
        }

        // DELETE api/<ReservaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Borrar(int id)
        {
            if (id == 0) return BadRequest("El id no puede ser 0");

            Reserva? reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound($"No se encontró ninguna reserva con el id {id}");
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return Ok("Reserva eliminada correctamente");
        }

        // Get api/<ReservaController>/cliente/5 Reservas de un cliente
        [HttpGet("cliente/{ciCliente}")]
        public async Task<IActionResult> ReservasCliente (int ciCliente)
        {
            if (ciCliente == 0) return BadRequest("La cédula del cliente no puede ser 0");

            List<Reserva> reservas = await _context.Reservas
                                                    .Include(r => r.ClientePagador)
                                                    .Include(r => r.Paquete)
                                                    .Include(r => r.Excursion)
                                                    .Where(r => r.ClientePagador.Ci == ciCliente)
                                                    .ToListAsync();
            if (reservas.Count == 0)
            {
                return NotFound($"No se encontraron reservas para el cliente con cédula {ciCliente}");
            }

            List<ReservaResponseDTO> reservasResponse = reservas.Select(r => new ReservaResponseDTO
            {
                Id = r.Id,
                IdClientePagador = r.IdClientePagador,
                IdExcursion = r.IdExcursion,
                IdPaquete = r.IdPaquete,
                MontoTotal = r.MontoTotal,
                EstadoReserva = r.EstadoReserva,
                FechaReserva = r.FechaReserva,
                ClientePagador = new ClienteResponseDTO
                {
                    Id = r.ClientePagador.Id
                },
                Paquete = new PaqueteResponseDTO
                {
                    Id = r.Paquete.Id,
                    Nombre = r.Paquete.Nombre,
                    Precio = r.Paquete.Precio
                },
                Excursion = new ExcursionesResponseDTO
                {
                    Id = r.Excursion.Id,
                    Nombre = r.Excursion.Nombre
                }
            }).ToList();

            return Ok(reservasResponse);
        }

        [HttpGet("excursion/{idExcursion}")]
        public async Task<IActionResult> getReservasDeExcursion(int? idExcursion)
        {
            if (idExcursion == 0) return BadRequest("El idExcursion no puede ser 0");

            List<ReservaListResponseDTO> reservas = await _context.Reservas
                                                            .Where(r => r.IdExcursion == idExcursion)
                                                            .Select(r => new ReservaListResponseDTO
                                                            {
                                                                Id = r.Id,
                                                                EstadoReserva = r.EstadoReserva,
                                                                NombreCliente = r.ClientePagador.Nombre,
                                                                ApellidoCliente = r.ClientePagador.Apellido,
                                                                CiCliente = r.ClientePagador.Ci
                                                            })
                                                            .ToListAsync();
            if (reservas.Count == 0)
            { return NotFound("No se encontraron reservas para esa excursion"); }

            return Ok(reservas);
        }

        [HttpGet("list")]
        public async Task<IActionResult> getReservasList()
        {
            List<ReservaListResponseDTO> reservas = await _context.Reservas
                                                            .Select(r => new ReservaListResponseDTO
                                                            {
                                                                Id = r.Id,
                                                                IdExcursion = r.IdExcursion,
                                                                EstadoReserva = r.EstadoReserva,
                                                                NombreCliente = r.ClientePagador.Nombre,
                                                                ApellidoCliente = r.ClientePagador.Apellido,
                                                                CiCliente = r.ClientePagador.Ci
                                                            })
                                                            .ToListAsync();
            if (reservas.Count == 0)
            { return NotFound("No se encontraron reservas para esa excursion"); }

            return Ok(reservas);
        }

        [HttpPut("agregar/{idRes}/{ciCliente}")]
        public async Task<IActionResult> agregarClienteAReserva(int idRes, int ciCliente)
        {
            if (idRes == 0 || ciCliente == 0) return BadRequest("Los ids no pueden ser igual a 0");

            Reserva? reserva = await _context.Reservas
                                                .Include(r => r.ClientesIncluidos)
                                                .Where(r => r.Id == idRes)
                                                .FirstOrDefaultAsync();
            
            if (reserva == null) return NotFound("No se encontro reserva con ese ID");
            

            Cliente? cliente = await _context.Clientes
                                                .Where(c => c.Ci == ciCliente)
                                                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound("No se encontro cliente con esa CI");

            if (reserva.ClientesIncluidos!.Find(c => c.Id == cliente.Id) != null) return BadRequest("El cliente ya esta agregado a la reserva");

            try
            {
                reserva.ClientesIncluidos.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok("Cliente agregado");

            } catch (DbUpdateException ex)
            {
                return Problem(detail:"No se pudo agregar al cliente",
                                title:"Error de Persistencia",
                                statusCode: StatusCodes.Status500InternalServerError);
            } 
            

            

        }
    }
}
