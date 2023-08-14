using FluentValidation;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.RabbitMq;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Api.Utils;
using LoginMicroservice.Domain.RedisRepositories;
using LoginMicroservice.Domain.Repositories;

namespace LoginMicroservice.Api.Services;

public class LoginServices : ILoginServices
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ITwoFactorRepository _twoFactorRepository;
    private readonly IValidator<LoginDto> _validator;
    private readonly IEmailSenderRabbitMq _emailSender;

    public LoginServices(IUserRepository userRepository, IValidator<LoginDto> validator, ITwoFactorRepository twoFactorRepository, IConfiguration configuration, IEmailSenderRabbitMq emailSender)
    {
        _userRepository = userRepository;
        _validator = validator;
        _twoFactorRepository = twoFactorRepository;
        _configuration = configuration;
        _emailSender = emailSender;
    }

    public async Task<LoginResponseDto> Call(LoginDto? value = null)
    {
        if (value == null)
        {
            throw new ValidationException("invalid DTO");
        }

        var validations = _validator.Validate(value);
        if (!validations.IsValid)
        {
            throw new ValidationException(validations.Errors);
        }

        var user = await _userRepository.LoginAsync(value.Email, value.Password);


        if (await _userRepository.IsTwoFactorLogin(value.Email))
        {
            Random random = new();
            var code = random.Next(100000, 999999).ToString();
            await _twoFactorRepository.SetAsync(code, user.Id.ToString());

            _emailSender.Execute(value.Email, code);
            return new LoginResponseDto();
        }

        return new LoginResponseDto(JwtGenerator.GenerateToken(_configuration, user));

    }
}
