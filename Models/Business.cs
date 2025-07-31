using System.Collections.Generic;

namespace CareBaseApi.Models
{
    public class Business
    {
        public int BusinessId { get; set; }  // chave primária nomeada
        public string Name { get; set; } = null!;
        public List<User> Users { get; set; } = new();
    }
}
