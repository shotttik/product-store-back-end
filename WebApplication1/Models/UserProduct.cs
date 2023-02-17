using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class UserProduct
    {
        public int ID_user { get; set; }

        public User User { get; set; }
        public int ID_product { get; set; }

        public Product Product { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
