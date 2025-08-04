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

        public List<User> Users { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public List<Patient> Patients { get; set; } = new(); // 👈 falta isso aqui

    }


}
