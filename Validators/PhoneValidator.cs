using System.Text.RegularExpressions;

namespace CareBaseApi.Validators
{
    public static class PhoneValidator
    {
        public static bool IsValid(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Aceita formatos com ou sem DDD, com ou sem espaços/traços/parênteses
            var cleaned = Regex.Replace(phone, @"[^\d]", ""); // remove tudo que não é número

            // Celular: 11 dígitos (ex: 11987654321), Fixo: 10 dígitos (ex: 1132654321)
            return cleaned.Length == 10 || cleaned.Length == 11;
        }
    }
}
