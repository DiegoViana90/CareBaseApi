using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;

namespace CareBaseApi.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;

        public ConsultationService(IConsultationRepository consultationRepository, IPatientRepository patientRepository)
        {
            _consultationRepository = consultationRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Consultation> CreateConsultationAsync(CreateConsultationRequestDTO dto)
        {
            var patient = await _patientRepository.FindPatientByIdAsync(dto.PatientId);
            if (patient == null)
                throw new ArgumentException("Paciente não encontrado.");

            var consultation = new Consultation
            {
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Local),
                EndDate = dto.EndDate.HasValue
                    ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Local)
                    : null,
                AmountPaid = dto.AmountPaid,
                Notes = dto.Notes,
                PatientId = dto.PatientId
            };

            return await _consultationRepository.AddAsync(consultation);
        }


        public async Task<IEnumerable<Consultation>> GetConsultationsByPatientAsync(int patientId)
        {
            return await _consultationRepository.GetByPatientIdAsync(patientId);
        }
        public async Task<IEnumerable<Consultation>> GetAllConsultationsByBuAsync(int businessId)
        {
            return await _consultationRepository.GetByBusinessIdAsync(businessId);
        }
    }
}
