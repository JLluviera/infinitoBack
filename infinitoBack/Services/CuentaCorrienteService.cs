using infinitoBack.Data;
using infinitoBack.Models;
using infinitoBack.Utils;

namespace infinitoBack.Services
{
    public class CuentaCorrienteService
    {
        private readonly AppDbContext _context;
        private readonly TransaccionesService _transaccionesService;

        public CuentaCorrienteService(AppDbContext context, TransaccionesService transaccionesService)
        {
            _context = context;
            _transaccionesService = transaccionesService;
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

        public decimal ConsultaSaldoDisponibleCliente(int idCliente)
        {
            decimal saldo = 0;
            return saldo;
        }

        public bool AgregarRestante(decimal restante) //Seria para cuendo queremos pagar una reserva con saldo pero el cliente tiene mas saldo del requerido. En la controladora acreditamos todas las transacciones y luego creamos una nueva transaccion con el restante
        {
            return true;
        }

        public bool CancelarReserva(int idReserva) // En este caso se crea transaccion que cancele las anteriores(si las hay) con el estado DevolucionPago
        {
            return true;
        }
    }
}
