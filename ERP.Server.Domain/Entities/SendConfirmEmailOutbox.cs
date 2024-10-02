using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;
public sealed class SendConfirmEmailOutbox : Entity
{
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public int TryCount { get; set; }

}
