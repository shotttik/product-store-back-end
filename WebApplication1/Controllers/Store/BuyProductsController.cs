using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.SqlClient;
using WebApplication1.Models;
using WebApplication1.Schemas.StoreSchemas;
using WebApplication1.Services.UserService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers.Store
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BuyProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        ResponseMessage response = new ResponseMessage();
        public BuyProductsController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<string> BuyProducts([FromBody] TransactionS transactions)
        {
            var userID = Int32.Parse(_userService.GetUserData());
            var dbUser = await _context.Users.FindAsync(userID);
            var sum = 0;

            List<int> productIDS = new List<int>();
            transactions.Products.ForEach(p => productIDS.Add(p.ID));

            var dbProducts = _context.Products.Where(product => productIDS.Contains(product.ID));

            if(dbUser == null)
            {
                response.Status = "Error";
                response.Message = "User not found";
                Response.StatusCode = 401;
                return JsonConvert.SerializeObject(response);
            }
            if(dbUser.Balance < transactions.Paid)
            {
                response.Status = "Error";
                response.Message = "You don't have enough balance";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            var somethingWrong = false;
            // getting prices from db to compare paid value
            await dbProducts.ForEachAsync(dbProduct =>
            {
                transactions.Products.ForEach(p =>
                {
                    if (p.ID == dbProduct.ID)
                    {
                        if(dbProduct.Quantity < p.Quantity)
                        {
                            somethingWrong = true;
                            return;
                        }
                        dbProduct.Quantity = dbProduct.Quantity - p.Quantity;
                        sum = (int)(sum + dbProduct.Price * p.Quantity);
                        // adding user product
                        /*var cmd = new SqlCommand(
                                 """
                                 INSERT INTO UserProducts (ID_user, ID_product, Quantity) values (@UserID, @ID, @Quantity)
                                """);
                        cmd.Parameters.Add(new SqlParameter("UserID", userID));
                        cmd.Parameters.Add(new SqlParameter("ID", p.ID));
                        cmd.Parameters.Add(new SqlParameter("Quantity", p.Quantity));

                        _context.Database.ExecuteSql(cmd);*/
                        Console.WriteLine(userID);
                        Console.WriteLine(p.ID);
                        _context.Database.ExecuteSql($"INSERT INTO UserProduct (ID_user, ID_product, Quantity) values ({userID}, {p.ID}, {p.Quantity})");
                    }
                });
            });
            // when user tries to buy more product than they are in db!
            if (somethingWrong)
            {
                response.Status = "Error";
                response.Message = "not enough product in db";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }
            //Coupon Check
            var dbCoupon = await _context.Coupon.SingleOrDefaultAsync(c => c.Code == transactions.Coupon);
            if(transactions.Coupon != "")
            {
                sum = sum * dbCoupon.Discount / 100;

            }
            // compare sum and paid
            if(sum != transactions.Paid)
            {
                response.Status = "Error";
                response.Message = "caluclated sum from db product prices and your paid price isnot correct :)))";
                Response.StatusCode = 400;
                return JsonConvert.SerializeObject(response);
            }

            //everything okay lets save to db
            dbUser.Balance -= transactions.Paid;
            Transaction t = new Transaction();
            t.Paid = transactions.Paid;
            t.User = dbUser;
            t.ProductS = transactions.Products.ToString();
            t.Coupon = dbCoupon;
            _context.Transaction.Add(t);

            await _context.SaveChangesAsync();

            response.Status = "Success";
            response.Message = "Congratulations, you have successfully purchased the products";
            Response.StatusCode = 200;
            return JsonConvert.SerializeObject(response);

        }
    }
}
