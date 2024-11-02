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

        public async Task<User> GetByUserName(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }

        public async Task<User> CreateAdmin(User user, List<string> roles, CancellationToken cancellationToken)
        {
            if (!roles.Contains("Admin"))
            {
                roles.Add("Admin");
            }

            foreach (var role in roles)
            {
                user.AddRole(role);
            }

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<User> AssignRole(Guid userId, string role, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            user.AddRole(role);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
