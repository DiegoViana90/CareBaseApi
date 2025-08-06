using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> CreatePatientAsync(CreatePatientRequestDTO dto, int businessId);

        Task<Patient?> GetPatientByCPFAsync(int businessId, string cpf);
        Task<IEnumerable<Patient>> GetPatientsByBusinessAsync(int businessId);
        Task<IEnumerable<PatientListDto>> GetPatientsWithLastConsultationAsync(int businessId);
    }
}
