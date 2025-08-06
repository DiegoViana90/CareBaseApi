using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<Consultation> CreateConsultationAsync(CreateConsultationRequestDTO dto);
        Task<IEnumerable<Consultation>> GetConsultationsByPatientAsync(int patientId);
        Task<IEnumerable<Consultation>> GetAllConsultationsByBuAsync(int businessId);
    }
}
