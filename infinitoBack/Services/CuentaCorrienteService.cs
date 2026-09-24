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

        private readonly ReservasService _reservasService;

        public CuentaCorrienteService(AppDbContext context, TransaccionesService transaccionesService, ReservasService reservasService)
        {
            _context = context;
            _transaccionesService = transaccionesService;
            _reservasService = reservasService;
        }

        public async Task<Resultado> AcreditarPagos(Reserva reserva) //Acredita las transacciones sobre la reserva que se esta anulando. Pasa el estado
        {
            decimal totalTransacciones = 0;

            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                totalTransacciones += transaccion.Monto;
            }

            Resultado res = await _transaccionesService.CrearSaldoCliente(reserva.Id, reserva.IdClientePagador, totalTransacciones);

            if (!res.Exitoso) return res;


            return Resultado.Correcto();
        }

        public async Task<decimal> ConsultaSaldoDisponibleCliente(Cliente cliente)
        {
            decimal saldo = 0;

            foreach (Transaccion transaccion in cliente.Transacciones)
            {
                if(transaccion.Estado == EstadoTransaccion.CreditoPorAnulacion || transaccion.Estado == EstadoTransaccion.UsoDeSaldo)
                {
                    saldo += transaccion.Monto;
                }
            }

            return saldo;
        }

        public async Task<Resultado> CancelarReserva(Reserva reserva) // En este caso se crea transaccion que cancele las anteriores(si las hay) con el estado DevolucionPago
        {
            decimal totalPago = await _reservasService.ConsultarTotalPagoAReserva(reserva);

            Transaccion transaccionCancelacion = new Transaccion();

            transaccionCancelacion.IdCliente = reserva.IdClientePagador;
            transaccionCancelacion.IdReserva = reserva.Id;
            transaccionCancelacion.Estado = EstadoTransaccion.CreditoPorAnulacion;
            transaccionCancelacion.FormaDePago = FormaDePago.Transferencia;
            transaccionCancelacion.Observaciones = $"Cancelacion de reserva {reserva.Id}";
            transaccionCancelacion.Monto = (totalPago * -1);
            
            try
            {
                await _context.Transacciones.AddAsync(transaccionCancelacion);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return Resultado.Error("No se pudo agregar la transaccion de cancelacion", TipoError.ErrorInesperado);
            }

            return Resultado.Correcto();
        }
    }
}
