using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.Products;

internal sealed class ProductQueryRepository : QueryRepository<Product>, IProductQueryRepository
{
    public ProductQueryRepository() : base("products")
    {
    }

    public async Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq("Name", name);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}