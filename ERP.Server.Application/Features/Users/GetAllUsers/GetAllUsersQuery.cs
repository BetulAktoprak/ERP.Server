using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.GetAllUsers;
public sealed record GetAllUsersQuery() : IRequest<Result<List<GetAllUsersQueryResponse>>>;
