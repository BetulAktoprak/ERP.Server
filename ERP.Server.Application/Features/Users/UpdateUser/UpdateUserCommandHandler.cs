using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.Users.UpdateUser;
internal sealed class UpdateUserCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("User not found");
        }

        if (user.UserName != request.UserName)
        {
            bool isUserNameExists = await userQueryRepository.IsNameExistsAsync(request.UserName, cancellationToken);

            if (isUserNameExists)
            {
                return Result<string>.Failure("Username already exists");
            }
        }

        if (user.Email != request.Email)
        {
            bool isEmailExists = await userQueryRepository.IsEmailExistsAsync(request.Email, cancellationToken);

            if (isEmailExists)
            {
                return Result<string>.Failure("Email already exists");
            }

            user.IsEmailConfirmed = false;
        }

        user = Mapper.Map<UpdateUserCommand, User>(request);

        userCommandRepository.Update(user);
        await outboxService.AddAsync(TableNames.User, OperationNames.Update, user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        //Eğer mail adresi değiştiyse
        //Onay maili göndereceğiz

        return "User update is successful";
    }
}
