namespace CareBaseApi.Dtos.Responses;
public class LoginResponseDTO
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Role { get; set; } = null!;
}
