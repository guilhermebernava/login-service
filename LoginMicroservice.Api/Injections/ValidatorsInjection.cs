using FluentValidation;
using LoginMicroservice.Api.Validators;
using LoginMicroservice.Api.Dtos;

namespace LoginMicroservice.Api.Injections;

public static class ValidatorsInjection
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    }
}
