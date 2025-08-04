namespace CareBaseApi.Dtos.Requests
{
    // CreateTestAccountRequestDTO.cs
    public class CreateTestAccountRequestDTO
    {
        public string BusinessName { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessTaxNumber { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserTaxNumber { get; set; }
    }

}