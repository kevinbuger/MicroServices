using Dapper;
using Discount.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var con = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
            var affected = await con.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var con = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
            var affected = await con.ExecuteAsync
                ("delete from Coupon where ProductName = @productName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var con = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
            var coupon = await con.QueryFirstOrDefaultAsync<Coupon> 
                ("select * from Coupon where ProductName = @productName",new { ProductName  = productName});
            
            if (coupon==null)
                return new Coupon {ProductName ="No Discount", Amount=0, Description="No Discount Desc" };
            
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var con = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
            var affected = await con.ExecuteAsync
                ("Update Coupon set  ProductName= @ProductName, Description= @Description, Amount= @Amount where Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount,Id=coupon.ID });

            if (affected == 0)
                return false;

            return true;
        }
    }
}