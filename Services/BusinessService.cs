using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBaseApi.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _repository;

        public BusinessService(IBusinessRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Business>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Business?> GetByIdAsync(int businessId)
        {
            return await _repository.GetByIdAsync(businessId);
        }

        public async Task<Business> CreateAsync(Business business)
        {
            // Aqui você pode adicionar validações de negócio
            await _repository.AddAsync(business);
            return business;
        }

        public async Task UpdateAsync(Business business)
        {
            var exists = await _repository.ExistsAsync(business.BusinessId);
            if (!exists)
                throw new KeyNotFoundException($"Empresa com id {business.BusinessId} não encontrada");

            await _repository.UpdateAsync(business);
        }

        public async Task DeleteAsync(int businessId)
        {
            var exists = await _repository.ExistsAsync(businessId);
            if (!exists)
                throw new KeyNotFoundException($"Empresa com id {businessId} não encontrada");

            await _repository.DeleteAsync(businessId);
        }

        public async Task<bool> ExistsAsync(int businessId)
        {
            return await _repository.ExistsAsync(businessId);
        }
    }
}
