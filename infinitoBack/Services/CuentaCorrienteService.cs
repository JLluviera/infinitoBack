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
                totalTransacciones += transaccion.Monto;
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
                    saldo += transaccion.Monto;
                }
            }

            return saldo;
        }

        public async Task<Resultado> CancelarReserva(Reserva reserva)
        {
            decimal totalPago = await _context.Transacciones
                .Where(transaccion => transaccion.IdReserva == reserva.Id)
                .SumAsync(transaccion => transaccion.Monto);

            Transaccion transaccionCancelacion = new Transaccion();

            transaccionCancelacion.IdCliente = reserva.IdClientePagador;
            transaccionCancelacion.IdReserva = reserva.Id;
            transaccionCancelacion.Estado = EstadoTransaccion.CreditoPorAnulacion;
            transaccionCancelacion.FormaDePago = FormaDePago.Transferencia;
            transaccionCancelacion.Observaciones = $"Cancelación de reserva {reserva.Id}";
            transaccionCancelacion.Monto = totalPago * -1;

            try
            {
                await _context.Transacciones.AddAsync(transaccionCancelacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                return Resultado.Error(
                    "No se pudo agregar la transacción de cancelación",
                    TipoError.ErrorInesperado
                );
            }

            return Resultado.Correcto();
        }
    }
}