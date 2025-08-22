// Models/PaymentInstallment.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBaseApi.Models
{
    public class PaymentInstallment
    {
        public int PaymentInstallmentId { get; set; }

        // FK -> Payment
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        // Número da parcela (1, 2, 3...)
        public int Number { get; set; }

        // Valor desta parcela
        [Column(TypeName = "numeric(12,2)")]
        public decimal Amount { get; set; }

        // Status de pagamento
        public bool IsPaid { get; set; } = false;

        // Data em que foi paga (se for o caso)
        public DateTime? PaidAt { get; set; }
    }
}
