using infinitoBack.Data;
using Microsoft.AspNetCore.Mvc;
using infinitoBack.Models;
using infinitoBack.DTOs;
using infinitoBack.Enum;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> ObtenerTransacciones()
        {
            List<Transaccion> transacciones = await _context.Transacciones.ToListAsync();
            return Ok(transacciones);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTransaccionesPorId(int id)
        {
            Transaccion? transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound($"No se encontro la transaccion con id {id}");
            }
            return Ok(transaccion);
        }
        [HttpPost]

        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionCrearDto transaccionDatos)
        {
            Transaccion transaccion = new Transaccion()
            {
                Monto = transaccionDatos.Monto,
                FechaCreacion = transaccionDatos.FechaCreacion,
                FormaDePAgo = transaccionDatos.FormaDePago,
                Observaciones = transaccionDatos.Observaciones,
                Estado = transaccionDatos.Estado,
                IdReserva = transaccionDatos.IdReserva,
                IdCliente = transaccionDatos.IdCliente,
            };

            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
            return Ok(transaccion);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarTransaccion(int id, [FromBody] TransaccionCrearDto transaccionEditada)
        {
            Transaccion? transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound($"No se encontro ninguna transaccion con el id {id}");
            }
            transaccion.Monto = transaccionEditada.Monto;
            transaccion.FechaCreacion = transaccionEditada.FechaCreacion;
            transaccion.FormaDePAgo = transaccionEditada.FormaDePago;
            transaccion.Observaciones = transaccionEditada.Observaciones;
            transaccion.Estado = transaccionEditada.Estado;
            transaccion.IdReserva = transaccionEditada.IdReserva;
            transaccion.IdCliente = transaccionEditada.IdCliente;

            await _context.SaveChangesAsync();
            return Ok($"La transaccion con id {id} se actualizo correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarTransaccion(int id)
        {
            Transaccion? transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound($"No se encontro ninguna transaccion con id {id}");
            }
            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();
            return Ok($"La transaccion con el id {id} se borro correctamente");
        }
    }
}