using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Validators;
using CareBaseApi.Dtos.Requests;

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

        public async Task<Business> CreateAsync(Business business)
        {
            // Aqui você pode adicionar validações de negócio
            await _businessRepository.AddAsync(business);
            return business;
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

        public async Task<Business> CreateBusinessAsync(CreateBusinessRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.BusinessName))
                throw new ArgumentException("Business name is required.");

            if (string.IsNullOrWhiteSpace(dto.BusinessTaxNumber))
                throw new ArgumentException("Business tax number is required.");

            if (!TaxNumberValidator.IsValid(dto.BusinessTaxNumber))
                throw new ArgumentException("Invalid tax number format.");

            if (string.IsNullOrWhiteSpace(dto.BusinessEmail))
                throw new ArgumentException("Business email is required.");

            // Validar se já existe empresa com o mesmo nome
            bool existsName = await _businessRepository.ExistsByNameAsync(dto.BusinessName);
            if (existsName)
                throw new ArgumentException("Business name already exists.");

            // Validar se já existe empresa com o mesmo email
            bool existsEmail = await _businessRepository.ExistsByEmailAsync(dto.BusinessEmail);
            if (existsEmail)
                throw new ArgumentException("Business email already exists.");

            // Validar se já existe empresa com o mesmo tax number
            bool existsTax = await _businessRepository.ExistsByTaxNumberAsync(dto.BusinessTaxNumber);
            if (existsTax)
                throw new ArgumentException("Business tax number already exists.");

            var business = new Business
            {
                Name = dto.BusinessName,
                TaxNumber = dto.BusinessTaxNumber,
                Email = dto.BusinessEmail,
                ExpirationDate = DateTime.UtcNow.AddYears(1)
            };

            await _businessRepository.AddAsync(business);

            return business;
        }
    }
}
