using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.DeleteUserById;
internal sealed class DeleteUserByIdCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<DeleteUserByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("User not found");
        }

        userCommandRepository.Delete(user);
        await outboxService.AddAsync(TableNames.User, OperationNames.Delete, user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "User delete is successful";
    }
}
