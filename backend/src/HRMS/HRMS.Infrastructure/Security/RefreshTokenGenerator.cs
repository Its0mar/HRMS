using HRMS.Application.Abstractions.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.Infrastructure.Security
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string Hash(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));

            return Convert.ToHexString(bytes);
        }
    }
}
