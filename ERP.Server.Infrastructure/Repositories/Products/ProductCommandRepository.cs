using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;

namespace ERP.Server.Infrastructure.Repositories.Products;

internal sealed class ProductCommandRepository(ApplicationDbContext context) : CommandRepository<Product>(context), IProductCommandRepository
{
}
