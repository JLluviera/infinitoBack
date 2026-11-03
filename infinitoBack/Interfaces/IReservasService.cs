using infinitoBack.Models;
using infinitoBack.Utils;

namespace infinitoBack.Interfaces
{
    public interface IReservasService
    {
        Task<Resultado> AnularReserva(int idReserva);

        Task<Resultado> CancelarReserva(int idReserva);

        Task<decimal> ConsultarTotalPagoAReserva(Reserva reserva);


    }
}
