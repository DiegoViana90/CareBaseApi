using System.ComponentModel.DataAnnotations;

namespace CareBaseApi.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(14)]
        public string? Cpf { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Profession { get; set; }

        public DateTime? LastConsultationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // 🔗 Relação com Business
        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        // 🔗 Relação com consultas
        public List<Consultation> Consultations { get; set; } = new();
    }
}
