using BCrypt.Net;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Enums;
using CareBaseApi.Dtos.Responses;
using Microsoft.AspNetCore.Identity.Data;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Validators;

namespace CareBaseApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IBusinessRepository _businessRepository;

        public UserService(IUserRepository repository, IBusinessRepository businessRepository)
        {
            _repository = repository;
            _businessRepository = businessRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Hash da senha antes de salvar
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Regra para atribuir role Admin ao primeiro usuário da empresa
            var existingUsers = await _repository.GetUsersByBusinessIdAsync(user.BusinessId);
            if (!existingUsers.Any())
                user.Role = UserRole.Admin;
            else if (user.Role != UserRole.Admin)
                user.Role = UserRole.User;

            await _repository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var exists = await _repository.ExistsAsync(user.UserId);
            if (!exists)
                throw new KeyNotFoundException($"Usuário com id {user.UserId} não encontrado");

            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Usuário com id {id} não encontrado");

            await _repository.DeleteAsync(id);
        }

        public async Task<User?> AuthenticateAsync(LoginRequestDTO loginRequestDTO)
        {
            // Validação básica
            if (!EmailValidator.IsValid(loginRequestDTO.Email) ||
                !PasswordValidator.IsValid(loginRequestDTO.Password))
                return null;

            // Busca o usuário pelo email
            var user = await _repository.GetByEmailAsync(loginRequestDTO.Email);
            if (user == null)
                return null;

            // Verifica se a senha está correta
            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password);
            if (!senhaValida)
                return null;

            return user;
        }

        public async Task<User> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO)
        {
            // 🔒 Validações
            if (!EmailValidator.IsValid(createUserRequestDTO.Email))
                throw new ArgumentException("Email inválido.");

            if (!PasswordValidator.IsValid(createUserRequestDTO.Password))
                throw new ArgumentException("Senha inválida.");

            if (!TaxNumberValidator.IsValid(createUserRequestDTO.TaxNumber) || createUserRequestDTO.TaxNumber.Length != 11)
                throw new ArgumentException("CPF inválido.");

            if (!TaxNumberValidator.IsValid(createUserRequestDTO.BusinessTaxNumber) || createUserRequestDTO.BusinessTaxNumber.Length != 14)
                throw new ArgumentException("Tax number da empresa (CNPJ) inválido.");

            var exists = await _businessRepository.ExistsByTaxNumberAsync(createUserRequestDTO.BusinessTaxNumber);
            if (!exists)
                throw new ArgumentException("Empresa com o tax number informado não existe.");

            // depois você pode buscar o business se precisar do ID:
            var business = await _businessRepository.GetAllAsync();
            var selectedBusiness = business.FirstOrDefault(b => b.TaxNumber == createUserRequestDTO.BusinessTaxNumber);
            if (selectedBusiness == null)
                throw new ArgumentException("Empresa não encontrada após validação.");

            var user = new User
            {
                Email = createUserRequestDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserRequestDTO.Password),
                TaxNumber = createUserRequestDTO.TaxNumber,
                BusinessId = selectedBusiness.BusinessId
            };


            var existingUsers = await _repository.GetUsersByBusinessIdAsync(user.BusinessId);
            user.Role = !existingUsers.Any() ? UserRole.Admin : UserRole.User;

            await _repository.AddAsync(user);
            return user;
        }

    }
}
