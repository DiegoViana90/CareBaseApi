using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<IEnumerable<Patient>> GetByBusinessIdAsync(int businessId);
        Task<Patient?> FindPatientByCpfAsync(int businessId, string cpf);
    }

}
