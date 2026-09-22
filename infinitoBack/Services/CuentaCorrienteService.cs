using infinitoBack.Data;

namespace infinitoBack.Services
{
    public class CuentaCorrienteService
    {
        private readonly AppDbContext _context;

        public CuentaCorrienteService(AppDbContext context)
        {
            _context = context;
        }

        public bool AnulacionReserva(int idReserva) //Acredita las transacciones sobre la reserva que se esta anulando. Pasa el estado
        {
            return true;
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
