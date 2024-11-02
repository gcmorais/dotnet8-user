using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
        Task<User> GetByUserName(string username, CancellationToken cancellationToken);
        Task<User> CreateAdmin(User user, List<string> roles, CancellationToken cancellationToken);
        Task<User> AssignRole(Guid userId, string role, CancellationToken cancellationToken);
    }
}
