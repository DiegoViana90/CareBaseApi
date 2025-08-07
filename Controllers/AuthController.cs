using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CareBaseApi.Models;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Dtos.Responses;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(loginRequestDTO);
                if (user == null)
                    return Unauthorized(new { message = "Email, senha ou status inválido." });

                var token = _tokenService.GenerateToken(user);

                var response = new LoginResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    BusinessName = user.Business.Name,
                    Role = user.Role.ToString()
                };

                return Ok(new
                {
                    message = "Login realizado com sucesso",
                    data = response
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // retorna JSON amigável ao front-end
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // loga e retorna erro genérico para o usuário
                return StatusCode(500, new { message = "Erro interno no servidor", details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var identity = HttpContext.User.Identity;

            if (identity == null || !identity.IsAuthenticated)
                return Unauthorized(new { message = "Token inválido ou expirado." });

            var claims = HttpContext.User.Claims;

            var response = new
            {
                Id = claims.FirstOrDefault(c => c.Type == "id")?.Value,
                Email = claims.FirstOrDefault(c => c.Type == "email")?.Value,
                Name = claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Role = claims.FirstOrDefault(c => c.Type == "role")?.Value
            };

            return Ok(new { data = response });
        }


    }
}
