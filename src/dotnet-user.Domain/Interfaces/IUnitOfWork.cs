namespace dotnet_user_api.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken);
    }
}
