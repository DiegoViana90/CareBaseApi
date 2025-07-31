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
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userService.AuthenticateAsync(loginRequestDTO);
            if (user == null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

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
