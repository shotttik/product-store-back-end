using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using WebApplication1.Schemas.StoreSchemas;

namespace WebApplication1.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]
    public class DelProductController : Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public DelProductController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public async Task<string> DelProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) {
            
                response.Status = "Error";
                response.Message = "Product not found";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }
            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();
            response.Status = "Success";
            response.Message = "Deleted successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }

    }
}
