using CareBaseApi.Models;
using Microsoft.EntityFrameworkCore;
using CareBaseApi.Data; // Add this line (replace with the actual namespace of AppDbContext if different)

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IConsultationRepository
    {
        Task<Consultation> AddAsync(Consultation consultation);
        Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Consultation>> GetByBusinessIdAsync(int businessId);
        Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId);
        Task AddOrUpdateDetailsAsync(ConsultationDetails details);
        Task<Consultation?> GetByIdAsync(int consultationId);

        // 🔽 ADICIONE ISSO:
        Task SaveChangesAsync();
        AppDbContext GetDbContext();
    }
}
