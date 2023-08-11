using LoginMicroservice.Domain.Helpers;

namespace LoginMicroservice.Domain.Tests.Helpers;

public class PasswordHelperTest
{
    public string Password { get; private set; } = "abc123";

    [Fact]
    public void ItShouldGeneratePasswordHash()
    {
        var dto = PasswordHelper.GeneratePassword(Password);
        Assert.True(dto != null);
        Assert.NotNull(dto.Salt);
        Assert.NotNull(dto.PasswordHash);
        Assert.True(dto.PasswordHash.Length > 20);
    }

    [Fact]
    public void ItShouldValidatePasswordHash()
    {
        var dto = PasswordHelper.GeneratePassword(Password);
        var isValidPassword = PasswordHelper.ValidatePassword(Password, dto.PasswordHash, dto.Salt);
        Assert.True(isValidPassword);
    }
}
