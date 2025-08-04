using CareBaseApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(int businessId);
        Task<Business> AddAsync(Business business);
        Task UpdateAsync(Business business);
        Task DeleteAsync(int businessId);
        Task<bool> ExistsAsync(int businessId);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByTaxNumberAsync(string taxNumber);
        Task DeactivateAsync(int businessId);

    }
}
