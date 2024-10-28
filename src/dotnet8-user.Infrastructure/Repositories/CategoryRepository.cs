using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using dotnet8_user.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_user.Infrastructure.Repositories
{
    internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
        public async Task<Category> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Category> GetByName(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<List<Category>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Set<Category>()
                .Include(c => c.User)
                .ToListAsync(cancellationToken);
        }
    }
}
