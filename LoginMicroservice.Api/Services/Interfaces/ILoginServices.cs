using LoginMicroservice.Api.Dtos;

namespace LoginMicroservice.Api.Services.Interfaces;

public interface ILoginServices : IServices<LoginResponseDto, LoginDto>
{
}
