using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Store
{
    [Route("api/storeProducts")]
    [ApiController]

    public class StoreProductController : ControllerBase
    {
        private IConfiguration Configuration;


        public StoreProductController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // GET: api/<StoreProductController>
        [HttpGet(Name = "GetStoreProducts")]
        public Object Get()
        {

            var a = this.Configuration.GetConnectionString("MyConnection");
            var conn = new SqlConnection(a);
            conn.Open();

            var cmd = new SqlCommand(
                "select * from Products",
                conn);
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
