using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ELTBackend.Utilities
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(IEnumerable<Claim> claims, double expiresInMinutes)
        {
            // Create key & credentials
            var key =
                _config["JwtSettings:Key"]
                ?? throw new InvalidOperationException("Jwt Key is missing in configuration!");
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256
            );

            // Create token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
