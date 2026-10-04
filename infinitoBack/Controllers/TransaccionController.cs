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
        public async Task<IActionResult> CrearTransaccionPago([FromBody] TransaccionCrearDto transaccionDatos)
        {
            Reserva? reserva = await _context.Reservas
                .Include(reserva => reserva.Transacciones)
                .FirstOrDefaultAsync(reserva =>
                    reserva.Id == transaccionDatos.IdReserva &&
                    reserva.IdClientePagador == transaccionDatos.IdCliente
                );

            if (reserva == null)
            {
                return BadRequest("No se encontró la reserva o no pertenece al cliente.");
            }

            decimal totalPagado = 0;

            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                if (
                    transaccion.Estado == EstadoTransaccion.Pago ||
                    transaccion.Estado == EstadoTransaccion.UsoDeSaldo
                )
                {
                    totalPagado += transaccion.Monto;
                }
            }

            decimal deudaActual = reserva.MontoTotal - totalPagado;

            if (deudaActual < 0)
            {
                deudaActual = 0;
            }

            // El pago supera lo que todavía debe la reserva
            if (transaccionDatos.Monto > deudaActual)
            {
                decimal montoPago = deudaActual;
                decimal excedente = transaccionDatos.Monto - deudaActual;

                // Si todavía había deuda, registramos la parte correspondiente como Pago
                if (montoPago > 0)
                {
                    Transaccion pago = new Transaccion
                    {
                        Monto = montoPago,
                        FechaCreacion = transaccionDatos.FechaCreacion,
                        FormaDePago = transaccionDatos.FormaDePago,
                        Observaciones = transaccionDatos.Observaciones,
                        Estado = EstadoTransaccion.Pago,
                        IdReserva = transaccionDatos.IdReserva,
                        IdCliente = transaccionDatos.IdCliente
                    };

                    await _context.Transacciones.AddAsync(pago);
                }

                // El resto queda como saldo a favor
                Transaccion credito = new Transaccion
                {
                    Monto = excedente,
                    FechaCreacion = transaccionDatos.FechaCreacion,
                    FormaDePago = transaccionDatos.FormaDePago,
                    Observaciones = $"Crédito generado por pago excedente de reserva {reserva.Id}",
                    Estado = EstadoTransaccion.CreditoPorPagoExcedente,
                    IdReserva = reserva.Id,
                    IdCliente = transaccionDatos.IdCliente
                };

                await _context.Transacciones.AddAsync(credito);

                await _context.SaveChangesAsync();

                return Ok(credito);
            }

            // Pago normal
            Transaccion transaccionNormal = new Transaccion
            {
                Monto = transaccionDatos.Monto,
                FechaCreacion = transaccionDatos.FechaCreacion,
                FormaDePago = transaccionDatos.FormaDePago,
                Observaciones = transaccionDatos.Observaciones,
                Estado = EstadoTransaccion.Pago,
                IdReserva = transaccionDatos.IdReserva,
                IdCliente = transaccionDatos.IdCliente
            };

            await _context.Transacciones.AddAsync(transaccionNormal);
            await _context.SaveChangesAsync();

            return Ok(transaccionNormal);
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
            transaccion.FormaDePago = transaccionEditada.FormaDePago;
            transaccion.Observaciones = transaccionEditada.Observaciones;
            transaccion.IdReserva = transaccionEditada.IdReserva;
            transaccion.IdCliente = transaccionEditada.IdCliente;
            transaccion.Estado=transaccionEditada.Estado;

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