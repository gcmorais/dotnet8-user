using dotnet_user_api.Domain.Entities;

namespace dotnet_user_api.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> Get(Guid id, CancellationToken cancellationToken);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
    }
}
