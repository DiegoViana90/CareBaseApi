using CareBaseApi.Enums;

namespace CareBaseApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string TaxNumber { get; set; } = null!; // ✅ CPF do usuário

        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
    }
}
