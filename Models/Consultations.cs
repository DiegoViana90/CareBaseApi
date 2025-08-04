using System.Text.Json.Serialization;

namespace CareBaseApi.Models
{
    public class Consultation
    {
        public int ConsultationId { get; set; }
        public DateTime Date { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Notes { get; set; }

        public int PatientId { get; set; }

        [JsonIgnore] // 👈 Isso evita o ciclo
        public Patient Patient { get; set; } = null!;
    }
}
