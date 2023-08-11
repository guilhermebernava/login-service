using LoginMicroservice.Api.Services;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Domain.Repositories;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.RedisRepositories;
using LoginMicroservice.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;

namespace LoginMicroservice.Api.Tests.Services;

public class TwoFactorLoginServicesTest
{

    private readonly ITwoFactorLoginServices _twoFactorLoginServices;
    private readonly IUserRepository _userRepository;
    private readonly User _userCorrect = new("a@a.com", "123456", false);

    public TwoFactorLoginServicesTest()
    {
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);
        var mockDatabase = new Mock<IDatabase>();
        mockMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);
        mockMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()).StringGetAsync(_userCorrect.Id.ToString(), CommandFlags.None)).Returns(Task.FromResult<RedisValue>(_userCorrect.Id.ToString()));
        var twoFactorRepository = new TwoFactorRepository(mockMultiplexer.Object);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT:Secret"]).Returns("7630b55d7c954cf493283886888427ec");
        configuration.Setup(x => x["JWT:ValidIssuer"]).Returns("localhost:7000");
        configuration.Setup(x => x["JWT:ValidAudience"]).Returns("localhost:7000");
        configuration.Setup(x => x["EmailPassword"]).Returns("raoutgjlgoewuewc");

        _userRepository = new UserRepository(new LoginContext(new DbContextOptionsBuilder<LoginContext>().UseInMemoryDatabase(databaseName: "UserTestDb").Options));
        _twoFactorLoginServices = new TwoFactorLoginServices(twoFactorRepository, configuration.Object, _userRepository);
    }

    [Fact]
    public async Task ItShouldTwoFactorLogin()
    {
        await _userRepository.CreateAsync(_userCorrect);
        var result = await _twoFactorLoginServices.Call(_userCorrect.Id.ToString());
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ItShouldNotTwoFactorLogin()
    {
        await _userRepository.CreateAsync(new User("a@a.com", "123456", false));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _twoFactorLoginServices.Call(""));
    }

}
