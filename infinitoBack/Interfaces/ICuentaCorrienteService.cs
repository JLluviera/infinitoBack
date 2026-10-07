using infinitoBack.Utils;
using infinitoBack.Models;

namespace infinitoBack.Interfaces
{
    public interface ICuentaCorrienteService
    {
        Task<Resultado> AcreditarPagos(Reserva reserva);

        Task<decimal> ConsultaSaldoDisponibleCliente(Cliente cliente);

        Task<Resultado> CancelarReserva(Reserva reserva);

        Task<decimal> ConsultarDeudaCliente(Cliente cliente);

        Task<Resultado> UsarSaldoEnReserva(int idCliente, int idReservaNueva, decimal monto);
    }
}
