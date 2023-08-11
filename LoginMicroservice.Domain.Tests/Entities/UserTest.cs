
using LoginMicroservice.Domain.Entities;

namespace LoginMicroservice.Domain.Tests.Entities;

public class UserTest
{

    [Fact]
    public void ItShouldCreateAnUser()
    {
        var user = new User("guilherme@test.com", "abc123", false);

        Assert.True(user.PasswordHash != "abc123");
        Assert.NotEmpty(user.PasswordSalt);
    }
}
