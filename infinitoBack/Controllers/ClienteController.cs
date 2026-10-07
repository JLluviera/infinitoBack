using infinitoBack.Data;
using infinitoBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using infinitoBack.DTOs;
using infinitoBack.Utils;
using infinitoBack.Services;
using Microsoft.Identity.Client;
using infinitoBack.Interfaces;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly ICuentaCorrienteService _cuentaCorrienteService;

        public ClienteController(AppDbContext context, ICuentaCorrienteService cuentaCorrienteService)
        {
            _context = context;
            _cuentaCorrienteService = cuentaCorrienteService;
        }

        [HttpGet]

        public async Task<IActionResult> ListarClientes()
        {
            List<Cliente> clientes = await _context.Clientes.ToListAsync();
            
            if (clientes.Count == 0)
            {
                return NotFound("No se encontraron clientes");
            }
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ClientePorId(int id)
        {
            Cliente? cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound($"No se encontro ningun cliente con el id {id}");
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteCrearDto datosCliente)
        {
            bool existe = await _context.Clientes.AnyAsync(cliente => cliente.Ci == datosCliente.Ci);
            if (existe)
            {
                return Conflict($"Ya hay un cliente con la cedula {datosCliente.Ci}");
            }
            Cliente cliente = new Cliente
            {
                Nombre = datosCliente.Nombre,
                Apellido = datosCliente.Apellido,
                Telefono = datosCliente.Telefono,
                Ci = datosCliente.Ci,
                FechaNacimiento = datosCliente.FechaNacimiento,
                FechaVencimientoCi = datosCliente.FechaVencimientoCi
            };

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return Ok("El cliente se registro con exito");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            Cliente? cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound($"No se encontro ningun cliente con id {id}");
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok($"El cliente con id {id} se elimino con exito");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarCliente(int id, [FromBody] ClienteCrearDto clienteModificado)
        {
            Cliente? cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound($"No se encontro ningun cliente con el id {id}");
            }
            bool existe = await _context.Clientes.AnyAsync(cliente => cliente.Id != id && cliente.Ci == clienteModificado.Ci);

            if (existe)
            {
                return Conflict("Ya existe otro cliente con esa cédula.");
            }

            cliente.Nombre = clienteModificado.Nombre;
            cliente.Apellido = clienteModificado.Apellido;
            cliente.Telefono = clienteModificado.Telefono;
            cliente.FechaNacimiento = clienteModificado.FechaNacimiento;
            cliente.Ci = clienteModificado.Ci;
            cliente.FechaVencimientoCi = clienteModificado.FechaVencimientoCi;

            await _context.SaveChangesAsync();
            return Ok($"Se modifico el cliente con el id {id}");
        }

        [HttpGet("ci/{ci}")]
        public async Task<IActionResult> GetClientePorCi(int ci)
        {
            Cliente? cliente = await _context.Clientes.Where(c => c.Ci == ci).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound("No se encontro Cliente con esa cédula");
            }

            return Ok(cliente);
        }

        [HttpGet("saldo/{idCliente}")]
        public async Task<IActionResult> GetSaldoDisponibleCliente(int idCliente)
        {
            if (idCliente <= 0) return BadRequest("Id de cliente inválido");

            Cliente? cliente = await _context.Clientes.Where(c => c.Id == idCliente)
                                                        .Include(c => c.Transacciones)
                                                        .FirstOrDefaultAsync();

            if (cliente == null) return NotFound("No se encontró cliente con esa Id");

            decimal saldo = await _cuentaCorrienteService.ConsultaSaldoDisponibleCliente(cliente);

            return Ok(saldo);
        }

        [HttpGet("deuda/{idCliente}")]
        public async Task<IActionResult> GetDeudaCliente(int idCliente)
        {
            if (idCliente <= 0) return BadRequest("Id de cliente inválido");

            Cliente? cliente = await _context.Clientes.Where(c => c.Id == idCliente)
                                                .Include(c => c.Transacciones)
                                                .Include(c => c.ReservasPagas)
                                                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound("No se encontró cliente con ese Id");

            decimal deuda = 0;

            deuda = await _cuentaCorrienteService.ConsultarDeudaCliente(cliente);

            return Ok(deuda);
        }

        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerClientesPaginado([FromQuery] int? idDespues)
        {
            int cantidad = 10;

            int ultimoId = idDespues ?? 0;

            List<Cliente> clientes = await _context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.Id > ultimoId)
                .OrderBy(cliente => cliente.Id)
                .Take(cantidad + 1)
                .ToListAsync();

            bool hayMas = clientes.Count > cantidad;

            if (hayMas)
            {
                clientes.RemoveAt(clientes.Count - 1);
            }

            int? siguienteCursor = null;

            if (clientes.Count > 0)
            {
                siguienteCursor = clientes[clientes.Count - 1].Id;
            }

            PaginaDTO<Cliente> pagina = new PaginaDTO<Cliente>
            {
                Elementos = clientes,
                SiguienteCursor = siguienteCursor,
                HayMas = hayMas
            };
            return Ok(pagina);
        }
    }
}