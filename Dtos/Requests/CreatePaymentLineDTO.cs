using CareBaseApi.Enums;
namespace CareBaseApi.Dtos.Requests
{
    public class CreatePaymentLineDTO
    {
        public PaymentMethod Method { get; set; }      // Pix, Debito, etc.
        public int Installments { get; set; } = 1;     // 1..12
        public decimal Amount { get; set; }            // R$ da linha
        public DateTime? PaidAt { get; set; }          // opcional
        public PaymentStatus Status { get; set; } = PaymentStatus.Paid;
        public string? ReferenceId { get; set; }
        public string? Notes { get; set; }
    }
}