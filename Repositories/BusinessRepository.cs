using CareBaseApi.Models;
using Microsoft.EntityFrameworkCore;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Data;


namespace CareBaseApi.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _context;

        public BusinessRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Business>> GetAllAsync()
        {
            return await _context.Business.Include(b => b.Users).ToListAsync();
        }

        public async Task<Business?> GetByIdAsync(int businessId)
        {
            return await _context.Business.Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.BusinessId == businessId);
        }

        public async Task<Business> AddAsync(Business business)
        {
            _context.Business.Add(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task UpdateAsync(Business business)
        {
            _context.Entry(business).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int businessId)
        {
            var business = await _context.Business.FindAsync(businessId);
            if (business != null)
            {
                _context.Business.Remove(business);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int businessId)
        {
            return await _context.Business.AnyAsync(b => b.BusinessId == businessId);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Business.AnyAsync(b => b.Name == name);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Business.AnyAsync(b => b.Email == email);
        }

        public async Task<bool> ExistsByTaxNumberAsync(string taxNumber)
        {
            return await _context.Business.AnyAsync(b => b.TaxNumber == taxNumber);
        }

        public async Task DeactivateAsync(int businessId)
        {
            var business = await _context.Business.FindAsync(businessId);
            if (business != null && business.IsActive)
            {
                business.IsActive = false;
                _context.Entry(business).Property(b => b.IsActive).IsModified = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}
