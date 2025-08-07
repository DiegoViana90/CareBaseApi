using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Dtos.Responses;

namespace CareBaseApi.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<Consultation> CreateConsultationAsync(CreateConsultationRequestDTO dto);
        Task<IEnumerable<ConsultationResponseDTO>> GetConsultationsByPatientWithNameAsync(int patientId);
        Task<IEnumerable<ConsultationResponseDTO>> GetAllConsultationsByBuAsync(int businessId);
        Task AddOrUpdateConsultationDetailsAsync(UpdateConsultationDetailsRequestDTO dto);
        Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId);
    }
}
