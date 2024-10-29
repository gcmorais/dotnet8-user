using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using dotnet8_user.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_user.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Categories)
                .ThenInclude(c => c.Products)
                .ToListAsync(cancellationToken);
        }
    }
}
