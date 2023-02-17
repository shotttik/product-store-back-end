using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Cryptography;
using WebApplication1.Schemas.Auth;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController :ControllerBase
    {
        private IConfiguration Configuration;


        public SignUpController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost(Name = "SignUp")]
        public string Post([FromBody] SignUp data)

        {

            ResponseMessage response = new ResponseMessage();
            if (data.Email == null || data.Password == null)
            {
                response.Status = "Error";
                response.Message = "Email or Password can't be null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            if (!Validations.IsValid(data.Email) || !Validations.IsValidPassword(data.Password))
            {
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
            var insertCommand = new SqlCommand(
                "insert into Users (Email, Level, PasswordHash, PasswordSalt, Balance) values (@Email, 0, @passwordHash, @passwordSalt, 100)",
                conn);

            try
            {
                cmd.Parameters.Add(new SqlParameter("Email", data.Email));
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string email = dr["Email"]?.ToString();
                    conn.Close();
                    response.Status = "Error";
                    response.Message = "This Email is already registered";
                    Response.StatusCode = 400;
                    return JsonConvert.SerializeObject(response);

                }
            }
            catch (SqlException)
            {

            }
            CreatePasswordHash(data.Password, out byte[] passwordHash, out byte[] passwordSalt);

            insertCommand.Parameters.Add(new SqlParameter("Email", data.Email));
            insertCommand.Parameters.Add(new SqlParameter("passwordHash", passwordHash));
            insertCommand.Parameters.Add(new SqlParameter("passwordSalt", passwordSalt));
            insertCommand.ExecuteNonQuery();

            conn.Close();
            response.Status = "Success";
            response.Message = "Account registered successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) { 
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        


    }
}
