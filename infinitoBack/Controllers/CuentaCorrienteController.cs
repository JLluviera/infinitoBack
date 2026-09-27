using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Models;
using infinitoBack.Services;
using infinitoBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaCorrienteController : ControllerBase
    {
        private readonly CuentaCorrienteService _cuentaCorrienteService;
        private readonly AppDbContext _context;

        public CuentaCorrienteController(
            CuentaCorrienteService cuentaCorrienteService,
            AppDbContext context)
        {
            _cuentaCorrienteService = cuentaCorrienteService;
            _context = context;
        }

        [HttpPost("usar-saldo")]
        public async Task<IActionResult> UsarSaldo(UsarSaldoDTO dto)
        {
            Resultado resultado = await _cuentaCorrienteService.UsarSaldoEnReserva(
                dto.IdCliente,
                dto.IdReservaNueva,
                dto.Monto
            );

            if (!resultado.Exitoso)
                return BadRequest(resultado);

            return Ok(resultado);
        }
        [HttpGet("saldo-disponible/{idCliente}")]
        public async Task<IActionResult> ConsultarSaldoDisponible(int idCliente)
        {
            Cliente? cliente = await _context.Clientes
                .Include(cliente => cliente.Transacciones)
                .FirstOrDefaultAsync(cliente => cliente.Id == idCliente);

            if (cliente == null)
            {
                return NotFound("No se encontró el cliente");
            }

            decimal saldo = await _cuentaCorrienteService
                .ConsultaSaldoDisponibleCliente(cliente);

            return Ok(new
            {
                idCliente = cliente.Id,
                saldoDisponible = saldo
            });
        }
    }
}
   