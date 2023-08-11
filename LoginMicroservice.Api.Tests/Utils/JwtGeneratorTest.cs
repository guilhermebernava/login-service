using LoginMicroservice.Api.Utils;
using LoginMicroservice.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LoginMicroservice.Api.Tests.Utils;

public class JwtGeneratorTest
{
    public readonly IConfiguration _configuration;

    public JwtGeneratorTest()
    {
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT:Secret"]).Returns("7630b55d7c954cf493283886888427ec");
        configuration.Setup(x => x["JWT:ValidIssuer"]).Returns("localhost:7000");
        configuration.Setup(x => x["JWT:ValidAudience"]).Returns("localhost:7000");
        _configuration = configuration.Object;
    }

    [Fact]
    public void ItShouldGenerateJwt()
    {
        var token = JwtGenerator.GenerateToken(_configuration, new User("a@a.com", "123456", false));
        Assert.NotNull(token);
    }
}
