// CareBaseApi/Models/Consultation.cs  (ATUALIZADO: remove AmountPaid e adiciona Payments)
using System.Text.Json.Serialization;
using CareBaseApi.Enums;

namespace CareBaseApi.Models
{
    public class Consultation
    {
        public int ConsultationId { get; set; }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set => _endDate = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Unspecified)
                : null;
        }

        public string? Notes { get; set; }
        public int PatientId { get; set; }
        public ConsultationStatus? Status { get; set; }

        [JsonIgnore]
        public Patient Patient { get; set; } = null!;

        [JsonIgnore]
        public ConsultationDetails? Details { get; set; }

        // 👇 navegação nova
        [JsonIgnore]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
