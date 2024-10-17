using dotnet_user_api.Domain.Entities;

namespace dotnet_user_api.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetById(Guid id, CancellationToken cancellationToken);
    }
}
