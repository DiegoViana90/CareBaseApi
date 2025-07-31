namespace CareBaseApi.Dtos.Responses
{
    public class CreateBusinessResponseDTO
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}