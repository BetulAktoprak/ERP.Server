using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;
public interface IProductCommandRepository : ICommandRepository<Product>
{
}

public interface IProductQueryRepository : IQueryRepository<Product>
{
    Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken = default);
}
