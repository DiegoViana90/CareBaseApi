// Dtos/Requests/UpdateConsultationDetailsRequestDTO.cs
namespace CareBaseApi.Dtos.Requests
{
    public class UpdateConsultationDetailsRequestDTO
    {
        public int ConsultationId { get; set; }

        public string? Titulo1 { get; set; }
        public string? Titulo2 { get; set; }
        public string? Titulo3 { get; set; }

        public string? Texto1 { get; set; }
        public string? Texto2 { get; set; }
        public string? Texto3 { get; set; }
    }
}
