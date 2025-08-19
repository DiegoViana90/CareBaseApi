using CareBaseApi.Data;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CareBaseApi.Dtos.Requests;

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
                .Include(p => p.Consultations) // 👈 Adiciona as consultas
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<PatientListDto>> GetSimplifiedWithLastConsultAsync(int businessId)
        {
            return await _context.Patients
                .Where(p => p.BusinessId == businessId && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PatientListDto
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Cpf = p.Cpf,
                    Phone = p.Phone,
                    Email = p.Email,
                    Profession = p.Profession,
                    CreatedAt = p.CreatedAt,
                    IsActive = p.IsActive,
                    BusinessId = p.BusinessId,

                    LastConsultation = p.Consultations
                        .OrderByDescending(c => c.StartDate)
                        .Select(c => new ConsultationDto
                        {
                            ConsultationId = c.ConsultationId,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            // ⬇️ somatório dos pagamentos
                            TotalPaid = c.Payments
                                .Select(x => (decimal?)x.Amount)
                                .Sum() ?? 0m,
                            Notes = c.Notes
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task<Patient?> FindPatientByCpfAsync(int businessId, string cpf)
        {
            return await _context.Patients
                .Where(p => p.BusinessId == businessId && p.Cpf == cpf)
                .FirstOrDefaultAsync();
        }
        public async Task<Patient?> FindPatientByIdAsync(int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

    }

}