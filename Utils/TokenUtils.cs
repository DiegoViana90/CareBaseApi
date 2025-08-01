using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace CareBaseApi.Utils
{
    public class TokenUtils
    {
        public static string? GetRoleFromToken(string token, string jwtSecret)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            if (token.StartsWith("Bearer "))
                token = token.Substring(7).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "role"
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var _);

                // ✅ Log de claims (útil para debug)
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"🔍 CLAIM: {claim.Type} = {claim.Value}");
                }

                return principal.Claims.FirstOrDefault(c =>
    c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro ao validar token: " + ex.Message);
                return null; // Token inválido
            }
        }
    }
}
