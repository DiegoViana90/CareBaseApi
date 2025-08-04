using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Validators;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Enums;
using CareBaseApi.Data;
using System.Security.Claims;

namespace CareBaseApi.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserService _userService;
        private readonly AppDbContext _dbContext; // usado apenas para controle de transação

        public BusinessService(
            IBusinessRepository businessRepository,
            IUserService userService,
            AppDbContext dbContext)
        {
            _businessRepository = businessRepository;
            _userService = userService;
            _dbContext = dbContext;
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

        public async Task<Business> CreateBusinessAsync(CreateBusinessDTO dto)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(dto.BusinessName))
                throw new ArgumentException("Nome da empresa é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.BusinessTaxNumber))
                throw new ArgumentException("CNPJ ou CPF da empresa é obrigatório.");

            if (!TaxNumberValidator.IsValid(dto.BusinessTaxNumber))
                throw new ArgumentException("CNPJ ou CPF inválido.");

            if (string.IsNullOrWhiteSpace(dto.BusinessEmail))
                throw new ArgumentException("Email da empresa é obrigatório.");

            // Verificações de unicidade
            if (await _businessRepository.ExistsByNameAsync(dto.BusinessName))
                throw new ArgumentException("Já existe uma empresa com esse nome.");

            if (await _businessRepository.ExistsByEmailAsync(dto.BusinessEmail))
                throw new ArgumentException("Já existe uma empresa com esse email.");

            if (await _businessRepository.ExistsByTaxNumberAsync(dto.BusinessTaxNumber))
                throw new ArgumentException("Já existe uma empresa com esse CNPJ/CPF.");

            // Definir expiração com base na role
            var expiration = dto.Role == UserRole.SM.ToString()
                ? DateTime.UtcNow.AddDays(31)
                : DateTime.UtcNow.AddDays(3);

            var business = new Business
            {
                Name = dto.BusinessName,
                TaxNumber = dto.BusinessTaxNumber,
                Email = dto.BusinessEmail,
                ExpirationDate = expiration
            };

            await _businessRepository.AddAsync(business);

            return business;
        }
        public async Task<Business> CreateTestAccountAsync(CreateTestAccountRequestDTO dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Cria empresa
                var business = await CreateBusinessAsync(new CreateBusinessDTO
                {
                    BusinessName = dto.BusinessName,
                    BusinessEmail = dto.BusinessEmail,
                    BusinessTaxNumber = dto.BusinessTaxNumber,
                    Role = null // padrão para teste
                });

                // Cria usuário vinculado à empresa
                await _userService.CreateUserAsync(new CreateUserRequestDTO
                {
                    Name = dto.UserName,
                    Email = dto.UserEmail,
                    Password = dto.UserPassword,
                    TaxNumber = dto.UserTaxNumber,
                    BusinessTaxNumber = dto.BusinessTaxNumber
                });

                await transaction.CommitAsync();

                return business;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Você pode logar se tiver ILogger:
                // _logger.LogError(ex, "Erro ao criar conta de teste");
                throw;
            }
        }

    }
}
