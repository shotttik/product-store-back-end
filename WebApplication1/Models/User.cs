using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Level { get; set; } = 0;
        public bool IsSuperUser { get; set; } = false;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;

        public List<UserProduct> Products { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
