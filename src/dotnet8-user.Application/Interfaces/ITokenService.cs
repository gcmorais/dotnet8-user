using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
