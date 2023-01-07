using Catalog.Model;
using MongoDB.Driver; 

namespace Catalog.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
