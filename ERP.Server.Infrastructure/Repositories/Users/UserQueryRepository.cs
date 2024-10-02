using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.Users;

internal sealed class UserQueryRepository : QueryRepository<User>, IUserQueryRepository
{
    public UserQueryRepository() : base("users")
    {
    }

    public async Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken)
    {
        var isDeletedFilter = Builders<User>.Filter.Eq(p => p.IsDeleted, false);
        var userNameOrEmailFilter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq(p => p.UserName, userNameOrEmail),
            Builders<User>.Filter.Eq(p => p.Email, userNameOrEmail)
        );
        var isActiveFilter = Builders<User>.Filter.Eq(p => p.IsActive, true);

        var combinedFilter = Builders<User>.Filter.And(isDeletedFilter, userNameOrEmail, isActiveFilter);
        return await _collection.Find(combinedFilter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Email", email);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string userName, CancellationToken cancellationToken)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("UserName", userName);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}
