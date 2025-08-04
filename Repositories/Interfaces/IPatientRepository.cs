using CareBaseApi.Models;

namespace CareBaseApi.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<Patient?> FindPatientByCpfAsync(int businessId, string cpf);
        Task<IEnumerable<Patient>> GetByBusinessIdAsync(int businessId); // ✅ Faltava esse
    }
}
