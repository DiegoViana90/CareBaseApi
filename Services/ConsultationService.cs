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
            // ✅ Corrigido: busca por ID
            var patient = await _patientRepository.FindPatientByIdAsync(dto.PatientId);
            if (patient == null)
                throw new ArgumentException("Paciente não encontrado.");

            var consultation = new Consultation
            {
                Date = dto.Date,
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
    }
}
