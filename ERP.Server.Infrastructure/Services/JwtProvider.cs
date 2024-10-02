using ERP.Server.Application.Services;
using ERP.Server.Domain.Entities;
using ERP.Server.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP.Server.Infrastructure.Services;
internal sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    public string CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("fullName", user.FullName),
            new Claim("email", user.Email)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler tokenHandler = new();

        string token = tokenHandler.WriteToken(jwtSecurityToken);

        return token;
    }
}
