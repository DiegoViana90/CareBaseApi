// Repositories/PaymentRepository.cs
using CareBaseApi.Data;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CareBaseApi.Enums;

namespace CareBaseApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context) => _context = context;

        public async Task AddRangeAsync(IEnumerable<Payment> payments)
        {
            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Payment>> GetByConsultationIdAsync(int consultationId)
        {
            return await _context.Payments
                                 .Where(p => p.ConsultationId == consultationId)
                                 .OrderBy(p => p.PaymentId)
                                 .ToListAsync();
        }

        public async Task<decimal> SumByConsultationIdAsync(int consultationId)
        {
            return await _context.Payments
                .Where(p => p.ConsultationId == consultationId &&
                            p.Status == PaymentStatus.Paid) 
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;
        }
    }
}
