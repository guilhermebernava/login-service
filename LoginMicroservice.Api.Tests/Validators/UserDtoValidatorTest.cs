using FluentValidation;
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Validators;

namespace LoginMicroservice.Api.Tests.Validators;

public class UserDtoValidatorTest
{
    private readonly IValidator<UserDto> _validtor = new UserDtoValidator();

    [Fact]
    public void ItShouldValidateUserDto()
    {
        var validations = _validtor.Validate(new UserDto("a@a.com", "123456", false));
        Assert.True(validations.IsValid);
    }

    [Fact]
    public void ItShouldShowErrorDueEmail()
    {
        var validations = _validtor.Validate(new UserDto("aa.com", "123456", false));
        Assert.False(validations.IsValid);
    }

    [Fact]
    public void ItShouldShowErrorDuePassword()
    {
        var validations = _validtor.Validate(new UserDto("a@a.com", "12345", false));
        Assert.False(validations.IsValid);
    }
}
