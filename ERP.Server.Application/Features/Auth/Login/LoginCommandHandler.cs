using ERP.Server.Application.Services;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler(
    IUserQueryRepository userQueryRepository,
    IJwtProvider jwtProvider
    ) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetUserByUserNameOrEmailAsync(request.UserNameOrEmail, cancellationToken);
        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("USer not found");
        }

        bool passwordIsTrue = HashingHelper.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash);

        if (!passwordIsTrue)
        {
            return Result<LoginCommandResponse>.Failure("Password is wrong");
        }
        if (!user.IsEmailConfirmed)
        {
            return Result<LoginCommandResponse>.Failure("Your email address not verify. You need to confirm first");
        }

        string token = jwtProvider.CreateToken(user);
        return new LoginCommandResponse("");
    }
}

