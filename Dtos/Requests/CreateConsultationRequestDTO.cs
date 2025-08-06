namespace CareBaseApi.Dtos.Requests
{
    public class CreateConsultationRequestDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
    }
}
