using infinitoBack.Models;
using infinitoBack.Utils;

namespace infinitoBack.Interfaces
{
    public interface ITransaccionesService
    {
        Task<Resultado> CrearSaldoCliente(int idReserva, int idCliente, decimal montoTotal);

        Task<Resultado> DevolucionPagos(Reserva reserva);

    }
}
