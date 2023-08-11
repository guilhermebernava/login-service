using AutoMapper;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Domain.Entities;

namespace LoginMicroservice.Api.Profiles;

public class MappersProfile : Profile
{
    public MappersProfile()
    {
        CreateMap<UserDto, User>();
    }
}
