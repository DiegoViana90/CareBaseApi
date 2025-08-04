using CareBaseApi.Models;
using CareBaseApi.Dtos.Requests;

namespace CareBaseApi.Services.Interfaces
{
    public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(int businessId);
        Task UpdateAsync(Business business);
        Task DeleteAsync(int businessId);
        Task<Business> CreateBusinessAsync(CreateBusinessDTO createBusinessDTO);
        Task<Business> CreateTestAccountAsync(CreateTestAccountRequestDTO dto);
    }
}
