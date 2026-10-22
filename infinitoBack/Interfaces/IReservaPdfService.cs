namespace infinitoBack.Services
{
    public interface IReservaPdfService
    {
        Task<byte[]> GenerarPdfAsync(int idReserva);
    }
}