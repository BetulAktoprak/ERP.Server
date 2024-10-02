using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.DeleteUserById;
public sealed record DeleteUserByIdCommand(
    Guid Id) : IRequest<Result<string>>;
