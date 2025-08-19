// Services/Interfaces/IConsultationService.cs
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Dtos.Responses;
using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<Consultation> CreateConsultationAsync(CreateConsultationRequestDTO dto);
        Task<IEnumerable<ConsultationResponseDTO>> GetConsultationsByPatientWithNameAsync(int patientId);
        Task<IEnumerable<ConsultationResponseDTO>> GetAllConsultationsByBuAsync(int businessId);
        Task AddOrUpdateConsultationDetailsAsync(UpdateConsultationDetailsRequestDTO dto);
        Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId);
        Task<List<Payment>> AddPaymentsAsync(int consultationId, List<CreatePaymentLineDTO> lines);
        Task<List<Payment>> GetPaymentsAsync(int consultationId);
        Task<decimal> GetTotalPaidAsync(int consultationId);
        Task<ConsultationDetailsFullResponseDTO?> GetDetailsFullAsync(int consultationId);
    }
}
