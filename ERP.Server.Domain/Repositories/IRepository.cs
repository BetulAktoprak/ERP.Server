using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Repositories;

public interface ICommandRepository<T> where T : Entity
{
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
}

public interface IQueryRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
}