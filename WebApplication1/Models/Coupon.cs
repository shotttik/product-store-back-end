using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Coupon
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
