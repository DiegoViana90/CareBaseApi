namespace CareBaseApi.Dtos.Requests
{
    public class ConsultationDto
    {
        public int ConsultationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Notes { get; set; }
    }
}