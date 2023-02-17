namespace WebApplication1
{
    public class ResponseMessage
    {
        public string? Status { get; set; }
        public string? Message { get; set; }

    }

    public class LoginResponseMessage: ResponseMessage
    {
        public int UserID { get; set; }
        public int Level { get; set; }
        public string? Token { get; set; }
    }

    public class CouponResponseMessage : ResponseMessage
    {
        public int? Discount { get; set; }
    }

}
