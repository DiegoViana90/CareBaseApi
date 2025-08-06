using CareBaseApi.Data;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly AppDbContext _context;

        public ConsultationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Consultation> AddAsync(Consultation consultation)
        {
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();
            return consultation;
        }

        public async Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Consultations
                .Where(c => c.PatientId == patientId)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Consultation>> GetByBusinessIdAsync(int businessId)
        {
            return await _context.Consultations
                .Include(c => c.Patient)
                .Where(c => c.Patient.BusinessId == businessId)
                .ToListAsync();
        }
    }
}
