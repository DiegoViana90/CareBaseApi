// Repositories/Interfaces/IPaymentInstallmentRepository.cs
using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IPaymentInstallmentRepository
    {
        Task<List<PaymentInstallment>> GetByPaymentIdAsync(int paymentId);
        Task MarkAsPaidAsync(int installmentId);
        Task AddRangeAsync(IEnumerable<PaymentInstallment> installments);
    }
}
