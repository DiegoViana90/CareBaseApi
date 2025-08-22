namespace CareBaseApi.Dtos.Requests
{

    public class PaymentLineDto
    {
        public string Method { get; set; } = null!;
        public int Installments { get; set; }
        public decimal Amount { get; set; }
        public List<PaymentInstallmentDto>? InstallmentsDetails { get; set; }
    }
}