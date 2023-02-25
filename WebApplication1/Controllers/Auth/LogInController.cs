using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Cryptography;
using WebApplication1.Controllers.Auth;
using WebApplication1.Models;
using WebApplication1.Schemas.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LogInController :ControllerBase

    {
        private IConfiguration Configuration;

        public LogInController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost(Name = "Login")]
        public string Post([FromBody] LogIn data)

        {

            LoginResponseMessage response = new LoginResponseMessage();
            if (data.Email == null || data.Password == null)
            {
                response.Status = "Error";
                response.Message = "Email or Password can't be null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            if(!Validations.IsValid(data.Email) || !Validations.IsValidPassword(data.Password)){
                response.Status = "Error";
                response.Message = "Email or Password validation failed";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            var cmd = new SqlCommand(
                $"select top(1) * from Users where Email=@Email",
                conn);
            cmd.Parameters.Add(new SqlParameter("Email", data.Email));


            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string email = dr["Email"].ToString()!;
                    byte[] passwordHash = (byte[])dr["PasswordHash"];
                    byte[] passwordSalt = (byte[])dr["PasswordSalt"];
                    if (!VerifyPasswordHash(data.Password, passwordHash, passwordSalt))
                    {
                        response.Status = "Error";
                        response.Message = "Email or Password is incorrect";
                        Response.StatusCode = 400;
                        return JsonConvert.SerializeObject(response);
                    }
                    int level = (int)dr["Level"];
                    int UserID = (int)dr["ID"];
                    bool issuperuser = (bool)dr ["IsSuperUser"];
                    decimal balance = (decimal)dr["balance"];
                    response.Level = level;
                    response.UserID = UserID;
                    string jwt = Token.CreateToken(UserID, email!, level, balance, issuperuser);
                    conn.Close();
                    response.Status = "Success";
                    response.Message = "Logged Successfully";
                    response.Token = jwt;
                    Response.StatusCode = 200;
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (SqlException)
            {

            }
            conn.Close();

            response.Status = "Error";
            response.Message = "Something Went Wrong";
            Response.StatusCode = 400;
            return JsonConvert.SerializeObject(response);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


    }
}
