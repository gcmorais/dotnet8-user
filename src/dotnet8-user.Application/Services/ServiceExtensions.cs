using System.Reflection;
using dotnet8_user.Application.Shared.Behavior;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.Create;
using dotnet8_user.Application.UseCases.CategoryUseCases.Update;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Update;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using dotnet8_user.Application.UseCases.UserUseCases.Update;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet8_user.Application.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationApp(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient<IValidator<CategoryCreateRequest>, CategoryValidator<CategoryCreateRequest>>();
            services.AddTransient<IValidator<CategoryUpdateRequest>, CategoryValidator<CategoryUpdateRequest>>();

            services.AddTransient<IValidator<UserCreateRequest>, UserValidator<UserCreateRequest>>();
            services.AddTransient<IValidator<UserUpdateRequest>, UserValidator<UserUpdateRequest>>();

            services.AddTransient<IValidator<ProductCreateRequest>, ProductValidator<ProductCreateRequest>>();
            services.AddTransient<IValidator<ProductUpdateRequest>, ProductValidator<ProductUpdateRequest>>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddProblemDetails();
        }
    }
}
