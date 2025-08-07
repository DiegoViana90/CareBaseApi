using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Responses;
using CareBaseApi.Enums; // 👈 Importante para ConsultationStatus
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;

        public ConsultationService(
            IConsultationRepository consultationRepository,
            IPatientRepository patientRepository)
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
                PatientName = c.Patient.Name,
                Status = c.Status ?? ConsultationStatus.Agendado

 // 👈 adiciona isso aqui também
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
                PatientName = c.Patient.Name,
                Status = c.Status ?? ConsultationStatus.Agendado
            });

        }

        public async Task AddOrUpdateConsultationDetailsAsync(UpdateConsultationDetailsRequestDTO dto)
        {
            var consultation = await _consultationRepository.GetByIdAsync(dto.ConsultationId);
            if (consultation == null)
                throw new ArgumentException("Consulta não encontrada.");

            // Atualiza status e valor pago, se vierem
            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (!Enum.TryParse<ConsultationStatus>(dto.Status, ignoreCase: true, out var statusEnum))
                    throw new ArgumentException("Status inválido.");

                consultation.Status = statusEnum;
            }

            if (dto.AmountPaid.HasValue)
                consultation.AmountPaid = (decimal)dto.AmountPaid.Value;

            // Inicia a transação
            var context = _consultationRepository.GetDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Atualiza ou insere detalhes
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

                // Salva tudo
                await _consultationRepository.SaveChangesAsync();

                await transaction.CommitAsync(); // ✅ commit
            }
            catch
            {
                await transaction.RollbackAsync(); // ❌ rollback em caso de erro
                throw;
            }
        }

        public async Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId)
        {
            var details = await _consultationRepository.GetDetailsByConsultationIdAsync(consultationId);

            if (details?.Consultation == null)
                return null; // ou lançar uma exceção, se preferir

            return details;
        }

    }
}
