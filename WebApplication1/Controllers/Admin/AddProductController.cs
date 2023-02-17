using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]

    public class AddProductController : Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public AddProductController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<string> AddUpdateProduct([FromBody] Product data)
        {
            if (data.ID == 0)
            {
                _context.Products.Add(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                var dbPoduct = await _context.Products.FindAsync(data.ID);

                if (dbPoduct == null) return JsonConvert.SerializeObject(response);

                dbPoduct.Name = data.Name;
                dbPoduct.Price = data.Price;
                dbPoduct.Quantity = data.Quantity;

                await _context.SaveChangesAsync();
            }

            return JsonConvert.SerializeObject(response);
        }
    }

}