namespace CareBaseApi.Validators
{
    public static class PasswordValidator
    {
        public static bool IsValid(string? password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            return password.Length >= 6; // mínimo 6 caracteres
        }
    }
}
