using Microsoft.IdentityModel.Tokens;
using CRUD.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRUD.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration _iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this._iconfiguration = iconfiguration;
        }
        public Tokens GenerateToken(string userName, List<string> roles)
        {
            return GenerateJWTTokens(userName,roles);
        }

        public Tokens GenerateRefreshToken(string username, List<string> roles)
        {
            return GenerateJWTTokens(username, roles);
        }

        public Tokens GenerateJWTTokens(string userName, List<string> roles)
        {
            /*try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
     new Claim(ClaimTypes.Name, userName)
                  }),
                    Expires = DateTime.Now.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();
                return new Tokens { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
            }
            catch (Exception ex)
            {
                return null;
            }*/
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
            };
            claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role, role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _iconfiguration["JWT:Issuer"],
                audience: _iconfiguration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials:credentials
                );
            var refreshToken = GenerateRefreshToken();
            return new Tokens { AccessToken =new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken };
        }



        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
            
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            /*var Key = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]);*/

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"])),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
