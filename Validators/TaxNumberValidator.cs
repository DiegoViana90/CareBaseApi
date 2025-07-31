using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CareBaseApi.Validators
{
    public static class TaxNumberValidator
    {
        public static bool IsValid(string taxNumber)
        {
            if (string.IsNullOrWhiteSpace(taxNumber))
                return false;

            var digits = Regex.Replace(taxNumber, @"[^\d]", "");

            if (digits.Length == 11)
                return IsValidCpf(digits);
            else if (digits.Length == 14)
                return IsValidCnpj(digits);

            return false;
        }

        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            // Invalid known CPF sequences
            var invalids = new[]
            {
                "00000000000", "11111111111", "22222222222", "33333333333",
                "44444444444", "55555555555", "66666666666", "77777777777",
                "88888888888", "99999999999"
            };

            if (invalids.Contains(cpf))
                return false;

            // Validate first digit
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (cpf[i] - '0') * (10 - i);

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            if (firstDigit != (cpf[9] - '0'))
                return false;

            // Validate second digit
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (cpf[i] - '0') * (11 - i);

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;

            return secondDigit == (cpf[10] - '0');
        }

        private static bool IsValidCnpj(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            // Invalid known CNPJ sequences
            var invalids = new[]
            {
                "00000000000000", "11111111111111", "22222222222222",
                "33333333333333", "44444444444444", "55555555555555",
                "66666666666666", "77777777777777", "88888888888888",
                "99999999999999"
            };

            if (invalids.Contains(cnpj))
                return false;

            int[] firstWeights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] secondWeights = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Validate first digit
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += (cnpj[i] - '0') * firstWeights[i];

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            if (firstDigit != (cnpj[12] - '0'))
                return false;

            // Validate second digit
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += (cnpj[i] - '0') * secondWeights[i];

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;

            return secondDigit == (cnpj[13] - '0');
        }
    }
}
