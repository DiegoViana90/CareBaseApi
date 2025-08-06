using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IConsultationRepository
    {
        Task<Consultation> AddAsync(Consultation consultation);
        Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Consultation>> GetByBusinessIdAsync(int businessId);
    }
}
