using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers.Store
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserProductController : ControllerBase
    {
        private IConfiguration Configuration;

        public UserProductController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet("{id}")]
        public Object Get(int id)
        {
            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            var cmd = new SqlCommand(
               """
               SELECT    Products.ID, Products.Name, Products.Price, UserProducts.Quantity
               FROM            Products INNER JOIN
                                        UserProducts ON Products.ID = UserProducts.ID_product INNER JOIN
                                        User ON UserProducts.ID_user = Users.ID
               WHERE Users.ID = @id
               """,
               conn);

            cmd.Parameters.Add(new SqlParameter("id", id));
            try
            {
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {                    
                    string jsonData = Utils.DataToJson(data);
                    conn.Close();
                    Response.StatusCode = 200;
                    return jsonData;
                }
            }
            catch (SqlException)
            {

            }
            conn.Close();

            ResponseMessage response = new ResponseMessage();
            response.Status = "Error";
            response.Message = "Something Went Wrong";
            Response.StatusCode = 400;
            return JsonConvert.SerializeObject(response);
        }
    }
}


