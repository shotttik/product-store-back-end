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

    public class UpdateUserProductController : ControllerBase
    {
        private IConfiguration Configuration;

        public UpdateUserProductController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost("{id}")]
        public string Post(int id, [FromBody] UserProductOperation data)
        {
            ResponseMessage response = new ResponseMessage();
            if (data.Operation == null)
            {
                response.Status = "Error";
                response.Message = "Operation can't be null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            string operation = "";
            if(data.Operation == "Add")
            {
                operation = "+";
            }
            else { operation = "-"; }

            var cmd = new SqlCommand(
                $"""
                UPDATE UserProducts

                SET UserProducts.Quantity = UserProducts.Quantity {operation} 1

                WHERE UserProducts.ID_user = @UserID AND  UserProducts.ID_product = @ID

                UPDATE Products

                SET Products.Quantity = Products.Quantity {(operation == "-" ? "+" : "-")} 1

                WHERE Products.ID = @ID
                """, conn);

            cmd.Parameters.Add(new SqlParameter("UserID", data.UserID));
            cmd.Parameters.Add(new SqlParameter("ID", id));
            cmd.ExecuteNonQuery();

            conn.Close();
            response.Status = "Success";
            response.Message = "Updated successfully";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);
        }

    }
}
