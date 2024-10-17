using dotnet_user_api.Domain.Entities;
using dotnet_user_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_user_api.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Products>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<Products> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await Context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
