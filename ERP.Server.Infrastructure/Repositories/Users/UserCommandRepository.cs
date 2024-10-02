using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;

namespace ERP.Server.Infrastructure.Repositories.Users;
internal sealed class UserCommandRepository : CommandRepository<User>, IUserCommandRepository
{
    public UserCommandRepository(ApplicationDbContext context) : base(context)
    {
    }
}
