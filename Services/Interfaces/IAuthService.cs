using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(LoginRequestDTO loginRequestDTO);
    }
}
