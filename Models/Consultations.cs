
namespace CareBaseApi.Models
{

    public class Consultation
    {
        public int ConsultationId { get; set; }
        public DateTime Date { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Notes { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
