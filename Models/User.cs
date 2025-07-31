namespace CareBaseApi.Models
{
    public class User
    {
        public int UserId { get; set; }  // chave primária nomeada
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // FK para Business
        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;
    }
}
