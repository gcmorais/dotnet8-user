using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetById(Guid id, CancellationToken cancellationToken);
        Task<Category> GetByName(string name, CancellationToken cancellationToken);
    }
}
