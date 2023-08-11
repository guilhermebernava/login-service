using LoginMicroservice.Api.Dtos;

namespace LoginMicroservice.Api.Services.Interfaces;

public interface ICreateUserServices : IServices<bool,UserDto>
{
}
