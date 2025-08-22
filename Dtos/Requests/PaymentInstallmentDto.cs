namespace CareBaseApi.Dtos.Requests
{
    public class PaymentInstallmentDto
    {
        public int Number { get; set; }
        public decimal Value { get; set; }   // 👈 mesmo nome do JSON
        public bool Paid { get; set; }       // 👈 mesmo nome do JSON
    }
}
