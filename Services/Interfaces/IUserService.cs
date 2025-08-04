using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace CareBaseApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO);

        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

}
