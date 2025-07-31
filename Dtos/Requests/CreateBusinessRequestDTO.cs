namespace CareBaseApi.Dtos.Requests
{
    public class CreateBusinessRequestDTO
    {
        public string BusinessName { get; set; } = null!;
        public string BusinessTaxNumber { get; set; } = null!; // CPF ou CNPJ
        public string BusinessEmail { get; set; } = null!;
    }
}