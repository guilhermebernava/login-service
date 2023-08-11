using LoginMicroservice.Api.Utils;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LoginMicroservice.Api.Tests.Utils;

public class EmailSenderTest
{
    private readonly IConfiguration _configuration;
    public EmailSenderTest()
    {
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["EmailPassword"]).Returns("raoutgjlgoewuewc");
        _configuration = configuration.Object;
    }

    [Fact]
    public void ItShouldSendEmail()
    {
        EmailSender.SendEmail("guilhermebernava@outlook.com", "123456", _configuration);
    }
}
