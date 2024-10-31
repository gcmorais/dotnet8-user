using dotnet8_user.Application.Interfaces;
using dotnet8_user.Application.Services;
using dotnet8_user.Domain.Interfaces;
using dotnet8_user.Infrastructure.Context;
using dotnet8_user.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet8_user.Infrastructure.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICreateVerifyHash, ServiceHash>();

        }
    }
}
