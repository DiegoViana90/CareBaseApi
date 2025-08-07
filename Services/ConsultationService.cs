using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Responses;

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

        public async Task<IEnumerable<ConsultationResponseDTO>> GetConsultationsByPatientWithNameAsync(int patientId)
        {
            var consultations = await _consultationRepository.GetByPatientIdAsync(patientId);

            return consultations.Select(c => new ConsultationResponseDTO
            {
                ConsultationId = c.ConsultationId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                PatientId = c.PatientId,
                PatientName = c.Patient.Name
            });
        }

        public async Task<IEnumerable<ConsultationResponseDTO>> GetAllConsultationsByBuAsync(int businessId)
        {
            var consultations = await _consultationRepository.GetByBusinessIdAsync(businessId);

            return consultations.Select(c => new ConsultationResponseDTO
            {
                ConsultationId = c.ConsultationId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                PatientId = c.PatientId,
                PatientName = c.Patient.Name
            });
        }

        public async Task AddOrUpdateConsultationDetailsAsync(UpdateConsultationDetailsRequestDTO dto)
        {
            var consultation = await _consultationRepository.GetByIdAsync(dto.ConsultationId);
            if (consultation == null)
                throw new ArgumentException("Consulta não encontrada.");

            var details = new ConsultationDetails
            {
                ConsultationId = dto.ConsultationId,
                Titulo1 = dto.Titulo1,
                Titulo2 = dto.Titulo2,
                Titulo3 = dto.Titulo3,
                Texto1 = dto.Texto1,
                Texto2 = dto.Texto2,
                Texto3 = dto.Texto3
            };

            await _consultationRepository.AddOrUpdateDetailsAsync(details);
        }
        public async Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId)
        {
            return await _consultationRepository.GetDetailsByConsultationIdAsync(consultationId);
        }
    }
}
