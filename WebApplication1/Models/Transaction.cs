using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Schemas.StoreSchemas;

namespace WebApplication1.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User User { get; set; }
        public string ProductS { get; set; }
        public int? CouponID { get; set; }
        public Coupon? Coupon { get; set; }
    }
}
