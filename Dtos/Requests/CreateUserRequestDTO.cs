namespace CareBaseApi.Dtos.Requests
{
    public class CreateUserRequestDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TaxNumber { get; set; } = null!; // ✅ CPF do usuário
        public string BusinessTaxNumber { get; set; } = null!; // CNPJ da empresa
        public string Name { get; set; } = null!;
    }
}
