using BCrypt.Net;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Enums;
using CareBaseApi.Dtos.Responses;
using Microsoft.AspNetCore.Identity.Data;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Validators;

namespace CareBaseApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Hash da senha antes de salvar
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Regra para atribuir role Admin ao primeiro usuário da empresa
            var existingUsers = await _repository.GetUsersByBusinessIdAsync(user.BusinessId);
            if (!existingUsers.Any())
                user.Role = UserRole.Admin;
            else if (user.Role != UserRole.Admin)
                user.Role = UserRole.User;

            await _repository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var exists = await _repository.ExistsAsync(user.UserId);
            if (!exists)
                throw new KeyNotFoundException($"Usuário com id {user.UserId} não encontrado");

            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Usuário com id {id} não encontrado");

            await _repository.DeleteAsync(id);
        }

        public async Task<User?> AuthenticateAsync(LoginRequestDTO loginRequestDTO)
        {
            // Validação básica
            if (!EmailValidator.IsValid(loginRequestDTO.Email) ||
                !PasswordValidator.IsValid(loginRequestDTO.Password))
                return null;

            // Busca o usuário pelo email
            var user = await _repository.GetByEmailAsync(loginRequestDTO.Email);
            if (user == null)
                return null;

            // Verifica se a senha está correta
            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password);
            if (!senhaValida)
                return null;

            return user;
        }

    }
}
