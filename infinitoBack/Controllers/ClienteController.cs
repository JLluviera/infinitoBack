using infinitoBack.Data;
using infinitoBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using infinitoBack.DTOs;
using infinitoBack.Utils;
using infinitoBack.Services;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly CuentaCorrienteService _cuentaCorrienteService;

        public ClienteController(AppDbContext context, CuentaCorrienteService cuentaCorrienteService)
        {
            _context = context;
            _cuentaCorrienteService = cuentaCorrienteService;
        }

        [HttpGet]

        public async Task<IActionResult> ListarClientes()
        {
            List<Cliente> clientes = await _context.Clientes.ToListAsync();
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
    }
}