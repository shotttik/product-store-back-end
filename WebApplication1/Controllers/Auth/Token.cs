using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Auth
{
    public class Token
    {

        public static string CreateToken(int id, string email, int level, decimal balance)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, level.ToString()),
                new Claim("Balance", balance.ToString()),

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                ConfigurationHelper.config.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken( 
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
