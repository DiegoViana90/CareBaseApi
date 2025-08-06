using CareBaseApi.Enums;

namespace CareBaseApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;

        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

        // Salva com o horário local, mas com DateTimeKind.Unspecified
        public DateTime CreatedAt { get; set; } =
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

        public bool IsActive { get; set; } = true;
    }
}
