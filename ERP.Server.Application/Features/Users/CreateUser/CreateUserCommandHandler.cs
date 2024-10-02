using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameExists = await userQueryRepository.IsNameExistsAsync(request.UserName, cancellationToken);

        if (isUserNameExists)
        {
            return Result<string>.Failure("Username already exists");
        }

        bool isEmailExists = await userQueryRepository.IsEmailExistsAsync(request.Email, cancellationToken);
        if (isEmailExists)
        {
            return Result<string>.Failure("Email already exists");
        }

        HashingHelper.CreatePassword(request.Password, out byte[] passwordSalt, out byte[] passwordHash);

        User user = Mapper.Map<CreateUserCommand, User>(request);

        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;

        await userCommandRepository.CreateAsync(user, cancellationToken);
        await outboxService.AddAsync(TableNames.User, OperationNames.Create, user.Id, cancellationToken);

        await outboxService.AddAsync()
        await unitOfWork.SaveChangesAsync(cancellationToken);

        //Onay maili göndereceğiz

        return "User create is successful";

    }
}

