// Dtos/Responses/PaymentLineResponseDTO.cs
namespace CareBaseApi.Dtos.Responses
{
    public class PaymentInstallmentResponseDTO
    {
        public int PaymentInstallmentId { get; set; }
        public int Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}