using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.Create
{
    public sealed class UserCreateMapper : Profile
    {
        public UserCreateMapper()
        {
            CreateMap<UserCreateRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
