using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication1.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserData()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var handler = new JwtSecurityTokenHandler();

                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
            }
            return result!;
        }
    }
}
