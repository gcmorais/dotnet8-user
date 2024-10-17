using dotnet_user_api.Domain.Entities;
using dotnet_user_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_user_api.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
        public async Task<Category> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await Context.Category.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
