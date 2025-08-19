// CareBaseApi/Models/Payment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareBaseApi.Enums;

namespace CareBaseApi.Models
{
    // Models/Payment.cs
    using System.Text.Json.Serialization;

    // Models/Payment.cs
    using System.Text.Json.Serialization;
    using CareBaseApi.Enums;

    public class Payment
    {
        public int PaymentId { get; set; }
        public int ConsultationId { get; set; }

        [JsonIgnore] // <- evita ciclo Payment -> Consultation -> Payments -> ...
        public Consultation Consultation { get; set; } = null!;

        public decimal Amount { get; set; }
        public int Installments { get; set; } = 1;
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Paid;
        public DateTime? PaidAt { get; set; }
        public string? ReferenceId { get; set; }
        public string? Notes { get; set; }
    }

}
