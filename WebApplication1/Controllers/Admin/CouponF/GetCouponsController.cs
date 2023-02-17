using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Admin.CouponF
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCouponsController : Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public GetCouponsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Coupon>>> GetCoupons()
        {
            var coupons = await _context.Coupon.ToListAsync();
            if(coupons==null) return NotFound();
            return Ok(coupons);
        }
    }
}
