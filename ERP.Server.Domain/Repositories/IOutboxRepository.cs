using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;
public interface IOutboxRepository : ICommandRepository<Outbox>
{
    Task<List<Outbox>> GetAllAsync(CancellationToken cancellationToken = default);
}
