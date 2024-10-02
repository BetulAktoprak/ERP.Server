using System.Text;

namespace ERP.Server.Application.Services;
public static class HashingHelper
{
    public static void CreatePassword(string password, out byte[] passwordSalt, out byte[] passwordHash)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != passwordHash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

}
