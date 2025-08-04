namespace CareBaseApi.Dtos.Requests
{
    public class CreatePatientRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Cpf { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Profession { get; set; }
        public DateTime? LastConsultationDate { get; set; }
        public int BusinessId { get; set; }
    }
}
