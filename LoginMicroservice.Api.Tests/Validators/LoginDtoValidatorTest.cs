using FluentValidation;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Validators;

namespace LoginMicroservice.Api.Tests.Validators;

public class LoginDtoValidatorTest
{
    private readonly IValidator<LoginDto> _validtor = new LoginDtoValidator();

    [Fact]
    public void ItShouldValidateLoginDto()
    {
        var validations = _validtor.Validate(new LoginDto("a@a.com", "123456"));
        Assert.True(validations.IsValid);
    }

    [Fact]
    public void ItShouldShowErrorDueEmail()
    {
        var validations = _validtor.Validate(new LoginDto("aa.com", "123456"));
        Assert.False(validations.IsValid);
    }

    [Fact]
    public void ItShouldShowErrorDuePassword()
    {
        var validations = _validtor.Validate(new LoginDto("a@a.com", "12345"));
        Assert.False(validations.IsValid);
    }
}
