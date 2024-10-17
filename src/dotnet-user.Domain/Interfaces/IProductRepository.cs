using dotnet_user_api.Domain.Entities;

namespace dotnet_user_api.Domain.Interfaces
{
    public interface IProductRepository : IBaseRepository<Products>
    {
        Task<Products> GetById(Guid id, CancellationToken cancellationToken);
    }
}
