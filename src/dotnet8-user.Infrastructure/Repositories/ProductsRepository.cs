using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using dotnet8_user.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_user.Infrastructure.Repositories
{
    internal class ProductsRepository : BaseRepository<Products>, IProductsRepository
    {
        public ProductsRepository(AppDbContext context) : base(context) { }
        public async Task<Products> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Products> GetByName(string name, CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
        public async Task<List<Products>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.User)
                .ToListAsync(cancellationToken);
        }
    }
}
