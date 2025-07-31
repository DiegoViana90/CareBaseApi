using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;

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
            // Pode fazer validações ou hashing da senha aqui
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
