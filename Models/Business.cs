using System;
using System.Collections.Generic;

namespace CareBaseApi.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string Name { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public DateTime? ExpirationDate { get; set; }

        // Salva com o horário local, mas com DateTimeKind.Unspecified
        public DateTime CreatedAt { get; set; } =
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

        public bool IsActive { get; set; } = true;

        public List<User> Users { get; set; } = new();
        public List<Patient> Patients { get; set; } = new();
    }
}
