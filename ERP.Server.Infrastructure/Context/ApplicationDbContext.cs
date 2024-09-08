using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.Server.Infrastructure.Context;
internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Outbox> Outboxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entires = ChangeTracker.Entries<Entity>();
        foreach (var entry in entires)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                .CurrentValue = DateTime.Now;
            }
            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt)
                    .CurrentValue = DateTime.Now;
                }
                entry.Property(p => p.UpdateAt)
                .CurrentValue = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
