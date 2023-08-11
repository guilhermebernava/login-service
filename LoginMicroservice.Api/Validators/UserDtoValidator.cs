using FluentValidation;
using LoginMicroservice.Api.Dtos;

namespace LoginMicroservice.Api.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(_ => _.Email).NotNull().NotEmpty().WithMessage("Email can not be null").EmailAddress().WithMessage("Must to be a valid email address");
        RuleFor(_ => _.Password).NotNull().NotEmpty().WithMessage("Password can not be null").MinimumLength(6).WithMessage("Password has to be minimum 6 charcteres");
        RuleFor(_ => _.TwoFactor).NotNull().WithMessage("TwoFactor can not be null");
    }
}
