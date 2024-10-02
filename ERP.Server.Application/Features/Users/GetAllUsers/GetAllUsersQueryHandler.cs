using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.Users.GetAllUsers;

internal sealed class GetAllUsersQueryHandler(
    IUserQueryRepository userQueryRepository) : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersQueryResponse>>>
{
    public async Task<Result<List<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<User> users =
            (await userQueryRepository.GetAllAsync(cancellationToken))
            .OrderBy(p => p.FirstName)
            .ToList();

        List<GetAllUsersQueryResponse> response =
            Mapper.Map<User, GetAllUsersQueryResponse>(users);

        return response;
    }
}
