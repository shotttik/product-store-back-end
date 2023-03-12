namespace WebApplication1.Schemas.AuthSchemas
{
    public class UserS
    {
        public int? ID { get; set; }
        public string Email { get; set; }
        public int Level { get; set; }
        public decimal Balance { get; set; }
        public bool isSuperUser { get; set; }
    }
}
