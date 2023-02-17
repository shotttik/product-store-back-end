﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using WebApplication1.Schemas.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/check")]
    [ApiController]
    public class CheckEmailController :ControllerBase

    {
        private IConfiguration Configuration;

        public CheckEmailController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost(Name = "Check")]
        public string Post([FromBody] Check data)

        {

            ResponseMessage response = new ResponseMessage();
            if (data.Email == null)
            {
                response.Status = "Error";
                response.Message = "Email or Password can't be null";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            if (!Validations.IsValid(data.Email))
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

            cmd.Parameters.Add(new SqlParameter("Email", data.Email));

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string email = dr["Email"].ToString();
                    conn.Close();
                    response.Status = "Success";
                    response.Message = "Checked successfully";
                    Response.StatusCode = 200;
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (SqlException)
            {

            }
            conn.Close();

            response.Status = "Error";
            response.Message = "Email don't exists";
            Response.StatusCode = 400;
            return JsonConvert.SerializeObject(response);
        }


    }
}
