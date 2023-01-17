using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                //var logger = services.GetRequiredService<ILogger>();
                try
                {
                    //logger.LogInformation("Start Migrating");
                    using var con = new NpgsqlConnection(
                        config.GetValue<string>("DatabaseSettings:ConnectionString"));
                     con.Open();
                    using var cmd = new NpgsqlCommand
                    {
                        Connection = con
                    };
                    cmd.CommandText = "DROP TABLE IF EXISTS Coupon"; 
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE Coupon( ID SERIAL PRIMARY KEY,
		                                                     ProductName     VARCHAR(24) NOT NULL,
		                                                     Description     TEXT,
		                                                     Amount          INT )";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);";
                    cmd.ExecuteNonQuery();

                    //logger.LogInformation("End Migrating");
                }
                catch (NpgsqlException ex)
                {
                    //logger.LogError(ex, "Migrating Error");
                    if(retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }return host;

        }
    }
}
