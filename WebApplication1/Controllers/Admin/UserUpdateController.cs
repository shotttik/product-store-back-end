using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Schemas.AuthSchemas;

namespace WebApplication1.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "SuperUser")]
    public class UserUpdateController :Controller
    {
        private readonly DataContext _context;
        ResponseMessage response = new ResponseMessage();
        public UserUpdateController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult> UserUpdate([FromBody] UserS user)
        {
            if (user == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();
            
            if(user.isSuperUser && user.Level != 1)
            {
                return BadRequest();
            }

            if(user.Balance < 0)
            {
                return BadRequest();

            }

            var db_user = await _context.Users.FindAsync(user.ID);
            if (db_user == null) 
                return BadRequest();

            db_user.Level = user.Level;
            db_user.Balance = user.Balance;
            db_user.IsSuperUser = user.isSuperUser;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
