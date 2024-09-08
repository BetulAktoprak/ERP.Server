using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;
public sealed class Outbox : Entity
{
    public string TableName { get; set; } = default!;
    public Guid RecordId { get; set; }
    public string OperationName { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public int TryCount { get; set; }
}
