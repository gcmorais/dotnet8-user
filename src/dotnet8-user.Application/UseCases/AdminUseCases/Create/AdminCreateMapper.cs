using AutoMapper;
using dotnet8_user.Application.UseCases.AdminUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.AdminUseCases.Create
{
    public sealed class AdminCreateMapper : Profile
    {
        public AdminCreateMapper()
        {
            CreateMap<AdminCreateRequest, User>();
            CreateMap<User, AdminResponse>();
        }
    }
}
