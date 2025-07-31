using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces;
public interface ITokenService
{
    string GenerateToken(User user);
}
