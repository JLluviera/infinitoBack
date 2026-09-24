using infinitoBack.Data;
using infinitoBack.Utils;
using System.Runtime.CompilerServices;
using infinitoBack.Enum;
using infinitoBack.Models;
using Microsoft.EntityFrameworkCore;

namespace infinitoBack.Services
{
    public class ReservasService
    {
        private readonly AppDbContext _context;
        private readonly CuentaCorrienteService _cuentaCorriente;

        public ReservasService (AppDbContext context, CuentaCorrienteService cuentaCorriente)
        {
            _context = context;
            _cuentaCorriente = cuentaCorriente;
        }

        public async Task<Resultado> AnularReserva(int idReserva)
        {
            Reserva? reserva = await _context.Reservas
                                    .Include(r => r.ClientePagador)
                                    .Include(r => r.Transacciones)
                                    .Where(r => r.Id == idReserva)
                                    .FirstOrDefaultAsync();

            if (reserva == null) {
                return Resultado.Error("No se encontró reserva con ese Id", TipoError.NoEncontrado); };

            if (!(reserva.EstadoReserva == EstadoRes.Pendiente)) return Resultado.Error("Solo se puede anular reservas pendientes", TipoError.ReglaDeNegocio);

            reserva.EstadoReserva = EstadoRes.Anulada;

            Resultado result = await _cuentaCorriente.AcreditarPagos(reserva);

            if (result.Exitoso)
            {
                await _context.SaveChangesAsync();
                return Resultado.Correcto();
            } else
            {
                return Resultado.Error("No se pudo anular la reserva", TipoError.ErrorInesperado);
            };
                
        }

        public async Task<Resultado> CancelarReserva(int idReserva)
        {
            Reserva? reserva = await _context.Reservas
                                            .Where(r => r.Id == idReserva)
                                            .Include(r => r.Transacciones)
                                            .FirstOrDefaultAsync();

            if (reserva == null) return Resultado.Error("No se encontró reserva con ese Id", TipoError.NoEncontrado);

            if (!(reserva.EstadoReserva == EstadoRes.Pendiente)) return Resultado.Error("Solo se pueden cancelar reservas pendientes", TipoError.ReglaDeNegocio);

            reserva.EstadoReserva = EstadoRes.Cancelada;

            if (reserva.Transacciones.Count > 0)
            {
                Resultado res = await _cuentaCorriente.CancelarReserva(reserva);

                if (!res.Exitoso)
                {
                    return Resultado.Error("No se pudo cancelar la reserva", TipoError.ErrorInesperado);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return Resultado.Error("No se pudo guardar los cambios", TipoError.ErrorInesperado);
            }
  
            return Resultado.Correcto();
        }

        public async Task<decimal> ConsultarTotalPagoAReserva(Reserva reserva)
        {
            decimal totalPago = 0;

            foreach (Transaccion transaccion in reserva.Transacciones)
            {
                totalPago += transaccion.Monto;
            }

            return totalPago; 
        }
    }
}
