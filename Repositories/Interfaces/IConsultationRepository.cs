using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IConsultationRepository
    {
        Task<Consultation> AddAsync(Consultation consultation);
        Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Consultation>> GetByBusinessIdAsync(int businessId);
        Task AddOrUpdateDetailsAsync(ConsultationDetails details);
        Task<Consultation?> GetByIdAsync(int consultationId);
        Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId);

    }
}
