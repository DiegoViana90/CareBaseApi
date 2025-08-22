// Services/ConsultationService.cs (adaptação)
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Responses;
using CareBaseApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPaymentRepository _paymentRepository; // 👈 novo

        public ConsultationService(
            IConsultationRepository consultationRepository,
            IPatientRepository patientRepository,
            IPaymentRepository paymentRepository)
        {
            _consultationRepository = consultationRepository;
            _patientRepository = patientRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<Consultation> CreateConsultationAsync(CreateConsultationRequestDTO dto)
        {
            var patient = await _patientRepository.FindPatientByIdAsync(dto.PatientId);
            if (patient == null)
                throw new ArgumentException("Paciente não encontrado.");

            var consultation = new Consultation
            {
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Local),
                EndDate = dto.EndDate.HasValue ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Local) : null,
                // AmountPaid: deixar de usar; se mantiver a coluna, ignore-a aqui
                Notes = dto.Notes,
                PatientId = dto.PatientId
            };

            return await _consultationRepository.AddAsync(consultation);
        }

        public async Task<IEnumerable<ConsultationResponseDTO>> GetConsultationsByPatientWithNameAsync(int patientId)
        {
            var consultations = await _consultationRepository.GetByPatientIdAsync(patientId);

            var result = new List<ConsultationResponseDTO>();
            foreach (var c in consultations)
            {
                var payments = await _paymentRepository.GetByConsultationIdAsync(c.ConsultationId);
                var total = payments.Sum(p => p.Amount);
                var paid = payments.Where(p => p.Status == PaymentStatus.Paid).Sum(p => p.Amount);

                result.Add(new ConsultationResponseDTO
                {
                    ConsultationId = c.ConsultationId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    PatientId = c.PatientId,
                    PatientName = c.Patient.Name,
                    Status = c.Status ?? ConsultationStatus.Agendado,
                    TotalValue = total,
                    AmountPaid = paid
                });
            }

            return result;
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

            // Atualiza status (ok manter aqui)
            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (!Enum.TryParse<ConsultationStatus>(dto.Status, true, out var statusEnum))
                    throw new ArgumentException("Status inválido.");
                consultation.Status = statusEnum;
            }

            // ❌ NÃO atualiza mais AmountPaid aqui (passa a vir da tabela Payments)

            var context = _consultationRepository.GetDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
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
                await _consultationRepository.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<ConsultationDetails?> GetDetailsByConsultationIdAsync(int consultationId)
            => _consultationRepository.GetDetailsByConsultationIdAsync(consultationId);

        // 👇 NOVOS MÉTODOS DE PAGAMENTO

        public async Task<List<Payment>> AddPaymentsAsync(int consultationId, List<CreatePaymentLineDTO> lines)
        {
            var consultation = await _consultationRepository.GetByIdAsync(consultationId);
            if (consultation == null)
                throw new ArgumentException("Consulta não encontrada.");

            if (lines == null || lines.Count == 0)
                throw new ArgumentException("Nenhuma linha de pagamento informada.");

            var payments = lines.Select(l => new Payment
            {
                ConsultationId = consultationId,
                Method = l.Method,
                Installments = l.Installments,
                Amount = l.Amount,
                PaidAt = l.PaidAt,
                Status = l.Status,
                ReferenceId = l.ReferenceId,
                Notes = l.Notes
            }).ToList();

            await _paymentRepository.AddRangeAsync(payments);
            return payments;
        }

        // Services/ConsultationService.cs
        public async Task<ConsultationDetailsFullResponseDTO?> GetDetailsFullAsync(int consultationId)
        {
            var consultation = await _consultationRepository.GetByIdAsync(consultationId);
            if (consultation == null) return null;

            var details = await _consultationRepository.GetDetailsByConsultationIdAsync(consultationId);
            var payments = await _paymentRepository.GetByConsultationIdAsync(consultationId);
            var totalPaid = await _paymentRepository.SumByConsultationIdAsync(consultationId);

            return new ConsultationDetailsFullResponseDTO
            {
                // blocos
                Titulo1 = details?.Titulo1,
                Titulo2 = details?.Titulo2,
                Titulo3 = details?.Titulo3,
                Texto1 = details?.Texto1,
                Texto2 = details?.Texto2,
                Texto3 = details?.Texto3,

                // consulta
                ConsultationId = consultation.ConsultationId,
                StartDate = consultation.StartDate,
                EndDate = consultation.EndDate,
                PatientId = consultation.PatientId,
                PatientName = consultation.Patient?.Name ?? string.Empty,
                Status = consultation.Status ?? ConsultationStatus.Agendado,

                // financeiro
                TotalPaid = totalPaid,

                // lista
                Payments = payments.Select(p => new PaymentLineResponseDTO
                {
                    PaymentId = p.PaymentId,
                    ConsultationId = p.ConsultationId,
                    Method = (int)p.Method,   // <- cast explícito
                    Installments = p.Installments,
                    Amount = p.Amount,
                    Status = (int)p.Status,   // <- cast explícito
                    PaidAt = p.PaidAt,
                    ReferenceId = p.ReferenceId,
                    Notes = p.Notes,
                }).ToList()
            };
        }

        public Task<List<Payment>> GetPaymentsAsync(int consultationId)
            => _paymentRepository.GetByConsultationIdAsync(consultationId);

        public Task<decimal> GetTotalPaidAsync(int consultationId)
            => _paymentRepository.SumByConsultationIdAsync(consultationId);
    }
}
