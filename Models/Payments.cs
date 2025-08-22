using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareBaseApi.Enums;
using System.Text.Json.Serialization;

namespace CareBaseApi.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int ConsultationId { get; set; }

        [JsonIgnore]
        public Consultation Consultation { get; set; } = null!;

        // Valor total
        public decimal Amount { get; set; }

        // Número de parcelas combinadas
        public int Installments { get; set; } = 1;

        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Paid;

        private DateTime? _paidAt;
        public DateTime? PaidAt
        {
            get => _paidAt;
            set => _paidAt = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Unspecified)
                : null;
        }

        public string? ReferenceId { get; set; }
        public string? Notes { get; set; }

        // 🔥 nova relação
        public List<PaymentInstallment> InstallmentsList { get; set; } = new();
    }
}
