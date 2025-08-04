using CareBaseApi.Data;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<IEnumerable<Patient>> GetByBusinessIdAsync(int businessId)
        {
            return await _context.Patients
                .Where(p => p.BusinessId == businessId && p.IsActive)
                .OrderByDescending(p => p.LastConsultationDate)
                .ToListAsync();
        }

        public async Task<Patient?> FindPatientByCpfAsync(int businessId, string cpf)
        {
            return await _context.Patients
                .Where(p => p.BusinessId == businessId && p.Cpf == cpf)
                .FirstOrDefaultAsync();
        }
    }

}