using CareBaseApi.Services.Interfaces;
using CareBaseApi.Models;
using CareBaseApi.Enums;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CareBaseApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;

        public TokenService(IConfiguration configuration)
        {
            _jwtSecret = configuration.GetSection("Jwt").GetValue<string>("Secret")
                ?? throw new ArgumentNullException(nameof(configuration), "JWT Secret not configured");
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, user.Role.ToString()),
             new Claim("BusinessId", user.BusinessId.ToString()) // <- aqui!
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(user.Role == UserRole.Admin ? 12 : 1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
