namespace CareBaseApi.Dtos.Requests
{
    public class PatientListDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Cpf { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Profession { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int BusinessId { get; set; }

        public ConsultationDto? LastConsultation { get; set; }
    }

}