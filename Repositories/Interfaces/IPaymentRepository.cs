using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddRangeAsync(IEnumerable<Payment> payments);
        Task<List<Payment>> GetByConsultationIdAsync(int consultationId);
        Task<decimal> SumByConsultationIdAsync(int consultationId);
    }
}
