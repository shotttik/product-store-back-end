namespace WebApplication1.Schemas.StoreSchemas
{
    public class UserProducts
    {
        public int? UserID { get; set; }
    }

    public class UserProductOperation: UserProducts
    {
        public string? Operation { get; set; }
    }
}
