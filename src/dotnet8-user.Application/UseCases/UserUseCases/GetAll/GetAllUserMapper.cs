using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetAll
{
    public class GetAllUserMapper : Profile
    {
        public GetAllUserMapper()
        {
            CreateMap<UserShortRequest, User>();
            CreateMap<User, UserShortResponse>();
        }
    }
}
