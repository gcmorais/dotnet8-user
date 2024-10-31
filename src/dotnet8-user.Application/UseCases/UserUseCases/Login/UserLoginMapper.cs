using AutoMapper;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.Login
{
    public sealed class UserLoginMapper : Profile
    {
        public UserLoginMapper()
        {
            CreateMap<UserLoginRequest, User>();
            CreateMap<User, UserLoginResponse>();
        }
    }
}
