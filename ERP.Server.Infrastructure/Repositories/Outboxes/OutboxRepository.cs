using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Server.Infrastructure.Repositories.Outboxes;
internal sealed class OutboxRepository : CommandRepository<Outbox>, IOutboxRepository
{
    private readonly ApplicationDbContext _context;
    public OutboxRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Outbox>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Outboxes
            .Where(p => p.IsCompleted == false)
            .OrderBy(p => p.CreateAt)
            .ToListAsync(cancellationToken);
    }
}
