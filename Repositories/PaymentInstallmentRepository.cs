// Repositories/PaymentInstallmentRepository.cs
using CareBaseApi.Data;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Repositories
{
    public class PaymentInstallmentRepository : IPaymentInstallmentRepository
    {
        private readonly AppDbContext _context;

        public PaymentInstallmentRepository(AppDbContext context) => _context = context;

        public async Task<List<PaymentInstallment>> GetByPaymentIdAsync(int paymentId)
        {
            return await _context.PaymentInstallments
                                 .Where(i => i.PaymentId == paymentId)
                                 .OrderBy(i => i.Number)
                                 .ToListAsync();
        }

        public async Task MarkAsPaidAsync(int installmentId)
        {
            var inst = await _context.PaymentInstallments.FindAsync(installmentId);
            if (inst != null)
            {
                inst.IsPaid = true;
                inst.PaidAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRangeAsync(IEnumerable<PaymentInstallment> installments)
        {
            await _context.PaymentInstallments.AddRangeAsync(installments);
            await _context.SaveChangesAsync();
        }
    }
}
