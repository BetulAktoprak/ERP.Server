using ERP.Server.Domain.Entities;

namespace ERP.Server.Application.Services;
public interface IJwtProvider
{
    string CreateToken(User user);
}
