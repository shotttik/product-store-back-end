using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Store
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckCouponController : Controller
    {
        private readonly DataContext _context;
        CouponResponseMessage response = new CouponResponseMessage();
        public CheckCouponController(DataContext context) { _context = context; }

        [HttpPost("{code}")]
        public async Task<string> Coupon(string code)
        {
            var dbCoupon = await _context.Coupon.SingleOrDefaultAsync(c => c.Code == code);
            if (dbCoupon == null)
            {
                response.Status = "Error";
                response.Message = "Coupon not found";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }
            if(dbCoupon.EndDate < DateTime.Now)
            {
                response.Status = "Error";
                response.Message = "Coupon expired";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            };
            if (dbCoupon.IsUsed)
            {
                response.Status = "Error";
                response.Message = "Coupon already used";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            };
            response.Status = "Success";
            response.Message = "Coupon found";
            response.Discount = dbCoupon.Discount;
            return JsonConvert.SerializeObject(response);
            
        }
    }
}
