using System;
using System.Collections.Generic;

namespace CareBaseApi.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string Name { get; set; } = null!;

        public string TaxNumber { get; set; } = null!;  // CPF ou CNPJ
        public string Email { get; set; } = null!;

        public DateTime? ExpirationDate { get; set; }

        public List<User> Users { get; set; } = new();
    }

}
