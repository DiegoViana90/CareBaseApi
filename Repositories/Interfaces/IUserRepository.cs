using CareBaseApi.Models;
using CareBaseApi.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int userId);
        Task<bool> ExistsAsync(int userId);
        Task<IEnumerable<User>> GetUsersByBusinessIdAsync(int businessId);
        Task<User?> GetByEmailAsync(string email);
    }
}
