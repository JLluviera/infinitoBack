using infinitoBack.Data;
using infinitoBack.Utils;
using System.Runtime.CompilerServices;
using infinitoBack.Enum;
using infinitoBack.Models;
using infinitoBack.Interfaces;

namespace infinitoBack.Services
{
    public class TransaccionesService : ITransaccionesService
    {
        private AppDbContext _context;

        public TransaccionesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado> CrearSaldoCliente(int idReserva, int idCliente, decimal montoTotal)
        {
            if (idReserva <= 0) return Resultado.Error("El Id Reserva no es válido", TipoError.ReglaDeNegocio);


            Transaccion transaccion = new Transaccion();

            transaccion.IdCliente = idCliente;
            transaccion.IdReserva = idReserva;
            transaccion.Estado = EstadoTransaccion.CreditoPorAnulacion;
            transaccion.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            transaccion.Monto = montoTotal;
            transaccion.Observaciones = "Creado automaticamente por anulacion de reserva";
            transaccion.FormaDePago = FormaDePago.Efectivo;

            try
            {
                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync();
                return Resultado.Correcto();
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo acreditar el saldo", TipoError.ErrorInesperado);
            }
        }

        public async Task<Resultado> DevolucionPagos(Reserva reserva)
        {
            decimal totalTransacciones = 0;
            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                if (
                    transaccion.Estado == EstadoTransaccion.Pago ||
                    transaccion.Estado == EstadoTransaccion.UsoDeSaldo || EstadoTransaccion.CreditoPorPagoExcedente == transaccion.Estado
                )
                {
                    totalTransacciones += transaccion.Monto;
                }
                else if (transaccion.Estado == EstadoTransaccion.DevolucionPago)
                {
                    totalTransacciones -= transaccion.Monto;
                }
            }

            Transaccion transaccionNueva = new Transaccion();

            transaccionNueva.IdCliente = reserva.IdClientePagador;
            transaccionNueva.IdReserva = reserva.Id;
            transaccionNueva.Estado = EstadoTransaccion.DevolucionPago;
            transaccionNueva.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            transaccionNueva.Monto = (totalTransacciones * -1);
            transaccionNueva.Observaciones = "Creado automaticamente por cancelación de reserva";
            transaccionNueva.FormaDePago = FormaDePago.Efectivo;

            try
            {
                _context.Transacciones.Add(transaccionNueva);
                await _context.SaveChangesAsync();
                return Resultado.Correcto();
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo devolver el saldo", TipoError.ErrorInesperado);
            }
        }
    }
}
