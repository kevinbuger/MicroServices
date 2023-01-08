using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            string conStr = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            string dbStr = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
            string colStr = configuration.GetSection("DatabaseSettings:CollectionName").Value;//configuration.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(conStr);
            var database = client.GetDatabase(dbStr); 
            Products = database.GetCollection<Product>(colStr);

            // CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
