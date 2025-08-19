namespace CareBaseApi.Dtos.Requests
{
    public class CreatePaymentsRequestDTO
    {
        public int ConsultationId { get; set; }
        public List<CreatePaymentLineDTO> Lines { get; set; } = new();
    }
}