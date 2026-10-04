using infinitoBack.Data;
using infinitoBack.Models;
using infinitoBack.Utils;
using infinitoBack.Enum;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Services
{
    public class CuentaCorrienteService
    {
        private readonly AppDbContext _context;
        private readonly TransaccionesService _transaccionesService;

        public CuentaCorrienteService(
            AppDbContext context,
            TransaccionesService transaccionesService)
        {
            _context = context;
            _transaccionesService = transaccionesService;
        }

        public async Task<Resultado> AcreditarPagos(Reserva reserva)
        {
            decimal totalTransacciones = 0;

            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                if (
                    transaccion.Estado == EstadoTransaccion.Pago ||
                    transaccion.Estado == EstadoTransaccion.UsoDeSaldo
                )
                {
                    totalTransacciones += transaccion.Monto;
                }
            }

            Resultado res = await _transaccionesService.CrearSaldoCliente(
                reserva.Id,
                reserva.IdClientePagador,
                totalTransacciones
            );

            if (!res.Exitoso)
                return res;

            return Resultado.Correcto();
        }

        public async Task<decimal> ConsultaSaldoDisponibleCliente(Cliente cliente)
        {
            decimal saldo = 0;

            foreach (Transaccion transaccion in cliente.Transacciones)
            {
                if (
                    transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion ||
                    transaccion.Estado == EstadoTransaccion.UsoDeSaldo
                )
                {
                    if (transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion)
                    {
                        saldo += transaccion.Monto;
                    }
                    else
                    {
                        saldo -= transaccion.Monto;
                    }
                }
            }

            return saldo;
        }

        public async Task<Resultado> CancelarReserva(Reserva reserva)
        {
            Resultado resultado = await AcreditarPagos(reserva);

            if (!resultado.Exitoso)
                return resultado;
            reserva.EstadoReserva = EstadoRes.Cancelada;

            try
            {
                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();

                return Resultado.Correcto();
            }
            catch (Exception excepcion)
            {
                return Resultado.Error(
                    "No se pudo cancelar la reserva",
                    TipoError.ErrorInesperado
                );
            }
        }

        public async Task<decimal> ConsultarDeudaCliente(Cliente cliente)
        {
            List<Reserva> reservas = await _context.Reservas
                .Include(reserva => reserva.Transacciones)
                .Where(reserva =>
                    reserva.IdClientePagador == cliente.Id &&
                    reserva.EstadoReserva == EstadoRes.Pendiente
                )
                .ToListAsync();

            decimal deuda = 0;

            foreach (Reserva reserva in reservas)
            {
                Console.WriteLine($"RESERVA {reserva.Id}");
                Console.WriteLine($"Monto total: {reserva.MontoTotal}");

                deuda += reserva.MontoTotal;

                foreach (Transaccion transaccion in reserva.Transacciones)
                {

                    if (
                        transaccion.Estado == EstadoTransaccion.Pago ||
                        transaccion.Estado == EstadoTransaccion.UsoDeSaldo
                    )
                    {
                        deuda -= transaccion.Monto;
                    }
                }

                Console.WriteLine($"Deuda acumulada: {deuda}");
            }

            return deuda;
        }

        public async Task<Resultado> UsarSaldoEnReserva(int idCliente,int idReservaNueva,decimal monto)
        {
            if (monto <= 0)
            {
                return Resultado.Error(
                    "El monto a utilizar debe ser mayor a cero",
                    TipoError.ReglaDeNegocio
                );
            }

            // Verificar que el cliente exista
            Cliente? cliente = await _context.Clientes
                .FirstOrDefaultAsync(cliente => cliente.Id == idCliente);

            if (cliente == null)
            {
                return Resultado.Error(
                    "No se encontró el cliente",
                    TipoError.NoEncontrado
                );
            }

            // Verificar que la reserva exista, pertenezca al cliente
            // y todavía esté pendiente
            Reserva? reserva = await _context.Reservas
                .FirstOrDefaultAsync(reserva =>
                    reserva.Id == idReservaNueva &&
                    reserva.IdClientePagador == idCliente &&
                    reserva.EstadoReserva == EstadoRes.Pendiente
                );

            if (reserva == null)
            {
                return Resultado.Error(
                    "La reserva no existe, no pertenece al cliente o no está pendiente",
                    TipoError.ReglaDeNegocio
                );
            }

            // Calcular saldo disponible del cliente
            decimal saldoDisponible = await _context.Transacciones
                .Where(transaccion =>
                    transaccion.IdCliente == idCliente &&
                    (
                        transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion ||
                        transaccion.Estado == EstadoTransaccion.UsoDeSaldo
                    )
                )
                .SumAsync(transaccion =>
                    transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion
                        ? transaccion.Monto
                        : -transaccion.Monto
                );

            // Verificar que tenga saldo suficiente
            if (monto > saldoDisponible)
            {
                return Resultado.Error(
                    "El cliente no posee saldo suficiente",
                    TipoError.ReglaDeNegocio
                );
            }

            // Buscar un crédito del cliente para dejarlo como referencia
            Transaccion? transaccionOrigen = await _context.Transacciones
                .FirstOrDefaultAsync(transaccion =>
                    transaccion.IdCliente == idCliente &&
                    transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion
                );

            if (transaccionOrigen == null)
            {
                return Resultado.Error(
                    "No se encontró una transacción de crédito disponible",
                    TipoError.NoEncontrado
                );
            }

            // Crear la nueva transacción
            Transaccion nuevaTransaccion = new Transaccion
            {
                IdCliente = idCliente,
                IdReserva = idReservaNueva,
                Monto = monto,
                Estado = EstadoTransaccion.UsoDeSaldo,
                FormaDePago = FormaDePago.Saldo,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Now),
                Observaciones =
                    $"Uso de saldo de la transacción {transaccionOrigen.Id}"
            };
            decimal totalPagado = await _context.Transacciones
    .Where(transaccion =>
        transaccion.IdReserva == idReservaNueva &&
        (
            transaccion.Estado == EstadoTransaccion.Pago ||
            transaccion.Estado == EstadoTransaccion.UsoDeSaldo
        )
    )
    .SumAsync(transaccion => transaccion.Monto);

            totalPagado += monto;

            if (totalPagado >= reserva.MontoTotal)
            {
                reserva.EstadoReserva = EstadoRes.Confirmada;
            }

            try
            {
                await _context.Transacciones.AddAsync(nuevaTransaccion);
                await _context.SaveChangesAsync();

                return Resultado.Correcto();
            }
            catch (Exception)
            {
                return Resultado.Error(
                    "No se pudo generar la transacción utilizando el saldo",
                    TipoError.ErrorInesperado
                );
            }
        }
    }
}