using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers.Admin.CouponF
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="1")]
    public class DelCouponController : Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public DelCouponController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("{id}")]
        public async Task<string> DelCoupon(int id)
        {
            var dbCoupon = await _context.Coupon.FindAsync(id);
            if(dbCoupon == null)
            {
                response.Status = "Error";
                response.Message = "Coupon not found";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }
            _context.Coupon.Remove(dbCoupon);
            await _context.SaveChangesAsync();
            response.Status = "Success";
            response.Message = "Deleted successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }
    }
}
