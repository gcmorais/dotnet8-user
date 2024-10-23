using System.Security.Cryptography;
using dotnet8_user.Domain.Interfaces;

namespace dotnet8_user.Application.Services
{
    public class ServiceHash : ICreateVerifyHash
    {
        public void CreateHashPassword(string password, out byte[] hashPassword, out byte[] saltPassword)
        {
            using (var HMAC = new HMACSHA512())
            {
                saltPassword = HMAC.Key;
                hashPassword = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool PasswordVerify(string password, byte[] hashPassword, byte[] saltPassword)
        {
            using (var HMAC = new HMACSHA512(saltPassword))
            {
                var computedHash = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hashPassword);
            }
        }
    }
}
