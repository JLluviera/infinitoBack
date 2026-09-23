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
    }
}
