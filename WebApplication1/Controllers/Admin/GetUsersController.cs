using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.UserService;

namespace WebApplication1.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "SuperUser")]
    public class GetUsersController :Controller
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        public GetUsersController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var userID = Int32.Parse(_userService.GetUserData());
            var users = await _context.Users
                    .Where(b => b.ID != userID)
                    .ToListAsync();
            return Ok(users);
        }
    }
}
