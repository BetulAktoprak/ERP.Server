using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;
public sealed class User : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public string Email { get; set; } = default!;
    public byte[] PasswordHash { get; set; } = [0];
    public byte[] PasswordSalt { get; set; } = [0];
    public bool IsEmailConfirmed { get; set; }
    public bool IsActive { get; set; } = true;
}
