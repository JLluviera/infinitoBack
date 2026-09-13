using infinitoBack.Models;

namespace infinitoBack.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);

    }
}
