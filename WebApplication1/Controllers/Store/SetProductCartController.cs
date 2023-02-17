using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using WebApplication1.Schemas.StoreSchemas;

namespace WebApplication1.Controllers.Store
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SetProductCartController : ControllerBase
    {
        private IConfiguration Configuration;

        public SetProductCartController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost("{id}")]
        public string Post(int id, [FromBody] UserProducts data)
        {
            ResponseMessage response = new ResponseMessage();
            if (data.UserID == 0)
            {
                response.Status = "Error";
                response.Message = "User Id Cannot be Null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }
            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            

            var cmd = new SqlCommand(
                """
                INSERT INTO UserProducts (ID_user, ID_product, Quantity) values (@UserID, @ID, 0)
                """, conn);

            cmd.Parameters.Add(new SqlParameter("UserID", data.UserID));
            cmd.Parameters.Add(new SqlParameter("ID", id));
            cmd.ExecuteNonQuery();

            conn.Close();
            response.Status = "Success";
            response.Message = "Added successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }

    }
}
