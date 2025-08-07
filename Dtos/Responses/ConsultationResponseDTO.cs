using CareBaseApi.Enums;
namespace CareBaseApi.Dtos.Responses

{
    public class ConsultationResponseDTO
    {
        public int ConsultationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public ConsultationStatus Status { get; set; } // 👈 ADICIONE ISSO
    }

}