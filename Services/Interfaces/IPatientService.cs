using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;

namespace CareBaseApi.Services.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> CreatePatientAsync(CreatePatientRequestDTO dto);
        Task<Patient?> GetPatientByCPFAsync(int businessId, string cpf);
    }
}
