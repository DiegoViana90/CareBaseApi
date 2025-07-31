using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Enums;

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
            // Verifica se já existe usuário na empresa
            var existingUsers = await _repository.GetUsersByBusinessIdAsync(user.BusinessId);
            if (!existingUsers.Any())
            {
                // Se for o primeiro usuário na empresa, define role Admin
                user.Role = UserRole.Admin;
            }
            else
            {
                // Caso contrário, mantém o role informado (ou define User por padrão)
                if (user.Role != UserRole.Admin)
                    user.Role = UserRole.User;
            }

            // Aqui você pode adicionar hash da senha, validações, etc
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
    }
}
