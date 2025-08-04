using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Validators;
using BCrypt.Net;

namespace CareBaseApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<User?> AuthenticateAsync(LoginRequestDTO loginRequestDTO)
        {
            if (!EmailValidator.IsValid(loginRequestDTO.Email) ||
                !PasswordValidator.IsValid(loginRequestDTO.Password))
                return null;

            var user = await _userRepository.GetByEmailAsync(loginRequestDTO.Email);
            if (user == null)
                return null;

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Usuário está inativo.");

            if (!user.Business.IsActive)
                throw new UnauthorizedAccessException("Empresa está inativa.");

            if (user.Business.ExpirationDate.HasValue &&
                user.Business.ExpirationDate.Value < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Empresa está expirada.");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password);
            if (!senhaValida)
                return null;

            return user;
        }

    }
}
