using System;
using System.Collections.Generic;

namespace CareBaseApi.Models
{
    public class Business
    {
        public int BusinessId { get; set; }  // chave primária nomeada
        public string Name { get; set; } = null!;

        public DateTime? ExpirationDate { get; set; } // data de expiração da empresa (opcional)

        public List<User> Users { get; set; } = new();
    }
}
