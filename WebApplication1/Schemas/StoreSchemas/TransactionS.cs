namespace WebApplication1.Schemas.StoreSchemas
{
    public class TransactionS
    {
        public decimal Paid { get; set; }
        public List<ProductS> Products { get; set; }
        public string Coupon { get; set; }

    }

    public class ProductS
    {
        public int ID { get; set; }
        public int Quantity { get; set; }

    }
}
