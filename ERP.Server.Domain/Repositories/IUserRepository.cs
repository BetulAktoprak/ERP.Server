using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;
public interface IUserCommandRepository : ICommandRepository<User>
{
}

public interface IUserQueryRepository : IQueryRepository<User>
{
    Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsNameExistsAsync(string userName, CancellationToken cancellationToken);
}
