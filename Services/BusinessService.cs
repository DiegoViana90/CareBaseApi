using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Validators;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Enums;
using System.Security.Claims;

namespace CareBaseApi.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessService(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public async Task<IEnumerable<Business>> GetAllAsync()
        {
            return await _businessRepository.GetAllAsync();
        }

        public async Task<Business?> GetByIdAsync(int businessId)
        {
            return await _businessRepository.GetByIdAsync(businessId);
        }

        public async Task UpdateAsync(Business business)
        {
            var exists = await _businessRepository.ExistsAsync(business.BusinessId);
            if (!exists)
                throw new KeyNotFoundException($"Empresa com id {business.BusinessId} não encontrada");

            await _businessRepository.UpdateAsync(business);
        }

        public async Task DeleteAsync(int businessId)
        {
            var exists = await _businessRepository.ExistsAsync(businessId);
            if (!exists)
                throw new KeyNotFoundException($"Empresa com id {businessId} não encontrada");

            await _businessRepository.DeleteAsync(businessId);
        }

        public async Task<bool> ExistsAsync(int businessId)
        {
            return await _businessRepository.ExistsAsync(businessId);
        }

        public async Task<Business> CreateBusinessAsync(CreateBusinessDTO createBusinessDTO)
        {
            if (string.IsNullOrWhiteSpace(createBusinessDTO.BusinessName))
                throw new ArgumentException("Business name is required.");

            if (string.IsNullOrWhiteSpace(createBusinessDTO.BusinessTaxNumber))
                throw new ArgumentException("Business tax number is required.");

            if (!TaxNumberValidator.IsValid(createBusinessDTO.BusinessTaxNumber))
                throw new ArgumentException("Invalid tax number format.");

            if (string.IsNullOrWhiteSpace(createBusinessDTO.BusinessEmail))
                throw new ArgumentException("Business email is required.");

            if (await _businessRepository.ExistsByNameAsync(createBusinessDTO.BusinessName))
                throw new ArgumentException("Business name already exists.");

            if (await _businessRepository.ExistsByEmailAsync(createBusinessDTO.BusinessEmail))
                throw new ArgumentException("Business email already exists.");

            if (await _businessRepository.ExistsByTaxNumberAsync(createBusinessDTO.BusinessTaxNumber))
                throw new ArgumentException("Business tax number already exists.");

            // ✅ Aqui sim calcula baseado na role recebida
            var expiration = createBusinessDTO.Role == UserRole.SM.ToString()
                ? DateTime.UtcNow.AddDays(31)
                : DateTime.UtcNow.AddDays(3);

            var business = new Business
            {
                Name = createBusinessDTO.BusinessName,
                TaxNumber = createBusinessDTO.BusinessTaxNumber,
                Email = createBusinessDTO.BusinessEmail,
                ExpirationDate = expiration
            };

            await _businessRepository.AddAsync(business);

            return business;
        }

    }
}
