using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.AdminUseCases.Assign
{
    public sealed class AssignAdminRoleMapper : Profile
    {
        public AssignAdminRoleMapper()
        {
            CreateMap<AssignAdminRoleRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
