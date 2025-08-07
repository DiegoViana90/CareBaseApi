using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBaseApi.Models
{
    public class ConsultationDetails
    {
        [Key]
        public int ConsultationDetailsId { get; set; }

        [Required]
        public int ConsultationId { get; set; }

        [ForeignKey(nameof(ConsultationId))]
        public Consultation Consultation { get; set; } = null!;

        public string? Titulo1 { get; set; }
        public string? Titulo2 { get; set; }
        public string? Titulo3 { get; set; }

        public string? Texto1 { get; set; }
        public string? Texto2 { get; set; }
        public string? Texto3 { get; set; }
    }
}
