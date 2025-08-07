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

        public async Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId)
        {
            return await _context.ConsultationDetails
                .Include(cd => cd.Consultation)
                .Where(cd => cd.Consultation != null && cd.ConsultationId == consultationId)
                .FirstOrDefaultAsync();

        }


        public async Task AddOrUpdateDetailsAsync(ConsultationDetails details)
        {
            var existing = await _context.ConsultationDetails
                .FirstOrDefaultAsync(d => d.ConsultationId == details.ConsultationId);

            if (existing != null)
            {
                existing.Titulo1 = details.Titulo1;
                existing.Titulo2 = details.Titulo2;
                existing.Titulo3 = details.Titulo3;
                existing.Texto1 = details.Texto1;
                existing.Texto2 = details.Texto2;
                existing.Texto3 = details.Texto3;
            }
            else
            {
                _context.ConsultationDetails.Add(details);
            }

            // Removido SaveChangesAsync daqui pois ele é chamado fora, na transação do service
        }

        public async Task<Consultation?> GetByIdAsync(int consultationId)
        {
            return await _context.Consultations.FirstOrDefaultAsync(c => c.ConsultationId == consultationId);
        }

        // ✅ Suporte para SaveChanges manual (controlado no service com transaction)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ✅ Expor o DbContext para controle de transações no service
        public AppDbContext GetDbContext()
        {
            return _context;
        }
    }
}
