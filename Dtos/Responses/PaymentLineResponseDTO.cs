// Dtos/Responses/PaymentLineResponseDTO.cs
namespace CareBaseApi.Dtos.Responses
{
    public class PaymentLineResponseDTO
    {
        public int PaymentId { get; set; }
        public int ConsultationId { get; set; }
        public int Method { get; set; }
        public int Installments { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? ReferenceId { get; set; }
        public string? Notes { get; set; }

        // 🔹 Nova propriedade
        public List<PaymentInstallmentResponseDTO>? InstallmentsDetails { get; set; }
    }
}
