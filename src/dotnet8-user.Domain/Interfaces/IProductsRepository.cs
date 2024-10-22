using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Domain.Interfaces
{
    public interface IProductsRepository : IBaseRepository<Products>
    {
        Task<Products> GetById(Guid id, CancellationToken cancellationToken);
        Task<Products> GetByName(string name, CancellationToken cancellationToken);
    }
}
