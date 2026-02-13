using System.Security.Claims;

namespace ELTBackend.Utilities
{
    public interface IJwtHelper
    {
        string GenerateToken(IEnumerable<Claim> claims, double expiresInMinutes);
    }
}
