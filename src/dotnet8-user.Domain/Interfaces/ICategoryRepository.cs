using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<User> GetById(int id, CancellationToken cancellationToken);
        Task<User> GetByName(string name, CancellationToken cancellationToken);
    }
}
