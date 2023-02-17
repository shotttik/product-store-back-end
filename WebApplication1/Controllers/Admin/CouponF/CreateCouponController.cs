using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Admin.CouponF
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]
    public class CreateCouponController : Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public CreateCouponController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<string> CreateCoupon([FromBody] Coupon coupon)
        {
            var c = _context.Coupon.SingleOrDefault(c => c.Code == coupon.Code);
            if (c != null)
            {
                Response.StatusCode = 401;
                return JsonConvert.SerializeObject(response);
            }
            _context.Coupon.Add(coupon);
            await _context.SaveChangesAsync();
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }
    }
}
