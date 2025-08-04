namespace CareBaseApi.Dtos.Requests
{
    public class CreateConsultationRequestDTO
    {
        public DateTime Date { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
    }
}
