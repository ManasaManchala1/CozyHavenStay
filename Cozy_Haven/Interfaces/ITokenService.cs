using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginUserDTO user);
    }
}
