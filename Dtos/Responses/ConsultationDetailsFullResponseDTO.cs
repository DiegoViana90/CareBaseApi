using CareBaseApi.Models;
using CareBaseApi.Enums;

namespace CareBaseApi.Dtos.Responses
{
    public class ConsultationDetailsFullResponseDTO
    {
        public string? Titulo1 { get; set; }
        public string? Titulo2 { get; set; }
        public string? Titulo3 { get; set; }
        public string? Texto1 { get; set; }
        public string? Texto2 { get; set; }
        public string? Texto3 { get; set; }
        public int ConsultationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";
        public ConsultationStatus Status { get; set; }
        public decimal TotalPaid { get; set; }
        public List<PaymentLineResponseDTO> Payments { get; set; } = new();
    }
}
