namespace CareBaseApi.Dtos.Requests
{
    public class CreatePaymentsRequestDTO
    {
        public int ConsultationId { get; set; }

        // 🔹 trocando CreatePaymentLineDTO por PaymentLineDto
        public List<PaymentLineDto> Lines { get; set; } = new();
    }

}