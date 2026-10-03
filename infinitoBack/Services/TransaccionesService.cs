using infinitoBack.Data;
using infinitoBack.Utils;
using System.Runtime.CompilerServices;
using infinitoBack.Enum;
using infinitoBack.Models;

namespace infinitoBack.Services
{
    public class TransaccionesService
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
    }
}
