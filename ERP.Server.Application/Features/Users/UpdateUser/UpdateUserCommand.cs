using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.UpdateUser;
public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email
    ) : IRequest<Result<string>>;

