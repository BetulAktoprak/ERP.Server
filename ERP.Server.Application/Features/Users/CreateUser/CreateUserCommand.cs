using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.CreateUser;
public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password
    ) : IRequest<Result<string>>;


