using CareBaseApi.Models;
using CareBaseApi.Dtos.Requests;

namespace CareBaseApi.Services.Interfaces
{
    public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(int businessId);
        Task<Business> CreateAsync(Business business);
        Task UpdateAsync(Business business);
        Task DeleteAsync(int businessId);
        Task<bool> ExistsAsync(int businessId);
        Task<Business> CreateBusinessAsync(CreateBusinessRequestDTO createBusinessRequestDTO);
    }
}
