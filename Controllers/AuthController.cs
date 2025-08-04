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
    }
}
