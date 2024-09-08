using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories;

internal class CommandRepository<T>(ApplicationDbContext context) : ICommandRepository<T> where T : Entity
{
    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
    }

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        context.Update(entity);
    }

    public void Update(T entity)
    {
        context.Update(entity);
    }
}

internal class QueryRepository<T> : IQueryRepository<T> where T : Entity
{
    public readonly IMongoCollection<T> _collection;

    public QueryRepository(string collectionName)
    {
        var client = new MongoClient("mongodb+srv://admin:1@erpdb.gkhkq.mongodb.net/");
        var database = client.GetDatabase("ERPDb");
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}
