using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBaseApi.Models
{
    public class PaymentInstallment
    {
        [Key]
        public int PaymentInstallmentId { get; set; }

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        public int Number { get; set; } // Número da parcela (1,2,3...)
        public decimal Amount { get; set; }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set => _dueDate = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
        }

        public bool IsPaid { get; set; }

        private DateTime? _paidAt;
        public DateTime? PaidAt
        {
            get => _paidAt;
            set => _paidAt = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Unspecified)
                : null;
        }
    }
}
