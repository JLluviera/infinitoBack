using infinitoBack.Interfaces;
using System.Runtime.CompilerServices;
namespace infinitoBack.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerificarPassword(string password, string passwordHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
