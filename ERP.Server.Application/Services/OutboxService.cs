using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;

namespace ERP.Server.Application.Services;
internal sealed class OutboxService(IOutboxRepository outboxRepository)
{
    public async Task AddAsync(string tableName, string operationName, Guid id, CancellationToken cancellationToken)
    {
        Outbox outbox = new()
        {
            TableName = tableName,
            OperationName = operationName,
            RecordId = id
        };
        await outboxRepository.CreateAsync(outbox, cancellationToken);
    }
}
