using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class Coupon
    {
        public int ID { get; set; }
        public string Code { get; set; } // how to make it unique
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }
        [AllowNull]
        public Transaction Transaction { get; set; }
        public Boolean IsUsed { get; set; } = false;

    }
}
