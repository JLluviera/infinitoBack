using infinitoBack.Data;
using infinitoBack.DTOs;
using infinitoBack.Enum;
using infinitoBack.Interfaces;
using infinitoBack.Models;
using infinitoBack.ParamsDTOs;
using infinitoBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICuentaCorrienteService _cuentaCorrienteService;

        public TransaccionController(
            AppDbContext context,
            ICuentaCorrienteService cuentaCorrienteService)
        {
            _context = context;
            _cuentaCorrienteService = cuentaCorrienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTransacciones([FromQuery] TransaccionesParamsDTO filtros)
        {
            IQueryable<Transaccion> consulta =
                _context.Transacciones.AsQueryable();

            if (filtros.IdCliente.HasValue)
            {
                consulta = consulta.Where(t =>
                    t.IdCliente == filtros.IdCliente.Value);
            }

            if (filtros.IdReserva.HasValue)
            {
                consulta = consulta.Where(t =>
                    t.IdReserva == filtros.IdReserva.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtros.NombreCliente))
            {
                consulta = consulta.Where(t =>
                    t.Cliente.Nombre.Contains(filtros.NombreCliente));
            }

            if (!string.IsNullOrWhiteSpace(filtros.ApellidoCliente))
            {
                consulta = consulta.Where(t =>
                    t.Cliente.Apellido.Contains(filtros.ApellidoCliente));
            }

            if (!string.IsNullOrWhiteSpace(filtros.BusquedaCliente))
            {
                string termino = filtros.BusquedaCliente.Trim();

                bool esNumero = int.TryParse(termino, out int ci);

                consulta = consulta.Where(t =>
                    t.Cliente.Nombre.Contains(termino) ||
                    t.Cliente.Apellido.Contains(termino) ||
                    (esNumero && t.Cliente.Ci == ci)
                );
            }

            List<Transaccion> transacciones =
                await consulta.ToListAsync();

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

            if (reserva.EstadoReserva == EstadoRes.Cancelada ||
                reserva.EstadoReserva == EstadoRes.Anulada)
            {
                return BadRequest("No se pueden registrar pagos en una reserva cancelada o anulada.");
            }

            if (transaccionDatos.Monto <= 0)
            {
                return BadRequest("El monto del pago debe ser mayor a cero.");
            }

            decimal totalPagado = 0;

            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                if (transaccion.Estado == EstadoTransaccion.Pago ||
                    transaccion.Estado == EstadoTransaccion.UsoDeSaldo)
                {
                    totalPagado += transaccion.Monto;
                }
                else if (transaccion.Estado == EstadoTransaccion.DevolucionPago)
                {
                    totalPagado -= transaccion.Monto;
                }
            }

            decimal deudaActual = Math.Max(0, reserva.MontoTotal - totalPagado);

            if (transaccionDatos.Monto > deudaActual)
            {
                decimal montoPago = deudaActual;
                decimal excedente = transaccionDatos.Monto - deudaActual;

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
            }
            else
            {
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
            }
            EstadoRes estadoAnterior = reserva.EstadoReserva;

            await _context.SaveChangesAsync();

            Resultado resultadoEstado =
                await _cuentaCorrienteService.ActualizarEstadoReserva(reserva.Id);

            if (!resultadoEstado.Exitoso)
            {
                return StatusCode(500, "No se pudo actualizar el estado de la reserva.");
            }

            bool reservaConfirmada =
                estadoAnterior == EstadoRes.Pendiente &&
                reserva.EstadoReserva == EstadoRes.Confirmada;

            return Ok(new
            {
                mensaje = "Transacción procesada con éxito",
                idReserva = reserva.Id,
                reservaConfirmada
            });
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