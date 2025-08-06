using CareBaseApi.Models;
using CareBaseApi.Dtos.Requests;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<Patient?> FindPatientByCpfAsync(int businessId, string cpf);
        Task<IEnumerable<Patient>> GetByBusinessIdAsync(int businessId);
        Task<Patient?> FindPatientByIdAsync(int patientId);
        Task<IEnumerable<PatientListDto>> GetSimplifiedWithLastConsultAsync(int businessId);
    }
}
