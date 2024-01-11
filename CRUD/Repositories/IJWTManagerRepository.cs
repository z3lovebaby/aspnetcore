using CRUD.Models;
using System.Security.Claims;

namespace CRUD.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens GenerateToken(string userName, List<string> roles);
        Tokens GenerateRefreshToken(string userName, List<string> roles);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
