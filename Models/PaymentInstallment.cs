// Models/PaymentInstallment.cs
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

        public DateTime DueDate { get; set; }   // 👈 faltava
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
