// Dtos/Requests/UpdateConsultationDetailsRequestDTO.cs
namespace CareBaseApi.Dtos.Requests
{
    public class UpdateConsultationDetailsRequestDTO
    {
        public int ConsultationId { get; set; }

        public string Titulo1 { get; set; } = string.Empty;
        public string Titulo2 { get; set; } = string.Empty;
        public string Titulo3 { get; set; } = string.Empty;

        public string Texto1 { get; set; } = string.Empty;
        public string Texto2 { get; set; } = string.Empty;
        public string Texto3 { get; set; } = string.Empty;

        public string? Status { get; set; }
        public double? AmountPaid { get; set; }
    }

}
