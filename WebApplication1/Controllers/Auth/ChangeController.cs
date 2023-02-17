using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using WebApplication1.Schemas.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/forgot")]
    [ApiController]
    public class ChangeController :ControllerBase
    {

        private IConfiguration Configuration;

        public ChangeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        [HttpPost(Name = "Forgot")]
        public string Post([FromBody] SignUp data)
        {
               
            ResponseMessage response = new ResponseMessage();
            if (data.Email == null || data.Password == null || data.ConfirmPassword == null)
            {
                response.Status = "Error";
                response.Message = "Email or Password can't be null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            if (!Validations.IsValid(data.Email) || !Validations.IsValidPassword(data.Password) || !Validations.IsValidPassword(data.ConfirmPassword))
            {
                response.Status = "Error";
                response.Message = "Email or Password validation failed";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            if (data.Password != data.ConfirmPassword)
            {
                response.Status = "Error";
                response.Message = "Passwords not match";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            var updateCommand = new SqlCommand(
                $"update Users set Password = @Password where Email = @Email",
                conn);
            updateCommand.Parameters.Add(new SqlParameter("Password", data.Email));
            updateCommand.Parameters.Add(new SqlParameter("Email", data.Email));

            updateCommand.ExecuteNonQuery();
            conn.Close();

            response.Status = "Success";
            response.Message = "Password changed successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }


    }
}
