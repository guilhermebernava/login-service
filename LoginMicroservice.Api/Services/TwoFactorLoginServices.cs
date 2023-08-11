using LoginMicroservice.Domain.RedisRepositories;
using LoginMicroservice.Domain.Repositories;
using FluentValidation;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Api.Utils;

namespace LoginMicroservice.Api.Services;

public class TwoFactorLoginServices : ITwoFactorLoginServices
{
    private readonly IUserRepository _userRepository;
    private readonly ITwoFactorRepository _twoFactorRepository;
    private readonly IConfiguration _configuration;

    public TwoFactorLoginServices(ITwoFactorRepository twoFactorRepository, IConfiguration configuration, IUserRepository userRepository)
    {
        _twoFactorRepository = twoFactorRepository;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<string> Call(string? value = null)
    {
        if (value == null)
        {
            throw new ValidationException("Null value");
        }
        var userId = await _twoFactorRepository.GetAsync(value) ?? throw new UnauthorizedAccessException("Invalid value");
        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
        return JwtGenerator.GenerateToken(_configuration, user);
    }
}
