namespace ERP.Server.Application.Features.Users.GetAllUsers;

public sealed record GetAllUsersQueryResponse(
    Guid Id,
    string FullName,
    string UserName,
    string Email,
    bool IsEmailConfirm,
    bool IsActive);
