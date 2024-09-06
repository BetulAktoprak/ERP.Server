using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;

namespace ERP.Server.Infrastructure.Repositories.Products;

internal sealed class ProductQueryRepository : QueryRepository<Product>, IProductQueryRepository
{
    public ProductQueryRepository() : base("products")
    {
    }
}