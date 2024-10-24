using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.Update
{
    public sealed class UpdateUserMapper : Profile
    {
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
