using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using CareBaseApi.Repositories.Interfaces;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Validators;

namespace CareBaseApi.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IBusinessRepository _businessRepository;

        public PatientService(IPatientRepository patientRepository, IBusinessRepository businessRepository)
        {
            _patientRepository = patientRepository;
            _businessRepository = businessRepository;
        }

        public async Task<Patient> CreatePatientAsync(CreatePatientRequestDTO dto, int businessId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Nome do paciente é obrigatório.");

            if (!await _businessRepository.ExistsAsync(businessId))
                throw new ArgumentException("Empresa informada não existe.");

            if (!string.IsNullOrWhiteSpace(dto.Cpf) && !TaxNumberValidator.IsValid(dto.Cpf))
                throw new ArgumentException("CPF inválido.");

            if (!string.IsNullOrWhiteSpace(dto.Phone) && !PhoneValidator.IsValid(dto.Phone))
                throw new ArgumentException("Telefone inválido.");

            if (!string.IsNullOrWhiteSpace(dto.Email) && !EmailValidator.IsValid(dto.Email))
                throw new ArgumentException("Email inválido.");

            var existingPatient = await GetPatientByCPFAsync(businessId, dto.Cpf);
            if (existingPatient != null)
                throw new ArgumentException("Já existe um paciente com o mesmo CPF para essa empresa.");

            var patient = new Patient
            {
                Name = dto.Name.Trim(),
                Cpf = dto.Cpf?.Trim(),
                Phone = dto.Phone?.Trim(),
                Email = dto.Email?.Trim(),
                Profession = dto.Profession?.Trim(),
                BusinessId = businessId,
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),

                IsActive = true
            };

            return await _patientRepository.AddAsync(patient);
        }

        public async Task<Patient?> GetPatientByCPFAsync(int businessId, string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF deve ser informado.");

            return await _patientRepository.FindPatientByCpfAsync(businessId, cpf);
        }

        public async Task<IEnumerable<Patient>> GetPatientsByBusinessAsync(int businessId)
        {
            return await _patientRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<PatientListDto>> GetPatientsWithLastConsultationAsync(int businessId)
        {
            var patientsRaw = await _patientRepository.GetSimplifiedWithLastConsultAsync(businessId);

            var result = patientsRaw.Select(p => new PatientListDto
            {
                PatientId = p.PatientId,
                Name = p.Name,
                Cpf = p.Cpf,
                Phone = p.Phone,
                Email = p.Email,
                Profession = p.Profession,
                CreatedAt = p.CreatedAt,
                IsActive = p.IsActive,
                BusinessId = p.BusinessId,
                LastConsultation = p.LastConsultation != null
                    ? new ConsultationDto
                    {
                        ConsultationId = p.LastConsultation.ConsultationId,
                        StartDate = p.LastConsultation.StartDate,
                        EndDate = p.LastConsultation.EndDate,
                        AmountPaid = p.LastConsultation.AmountPaid,
                        Notes = p.LastConsultation.Notes
                    }
                    : null
            });

            return result;
        }


    }
}

