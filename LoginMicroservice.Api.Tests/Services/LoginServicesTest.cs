using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Services;
using LoginMicroservice.Api.Services.Interfaces;
using LoginMicroservice.Api.Validators;
using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Domain.Repositories;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.Exceptions;
using LoginMicroservice.Infra.RedisRepositories;
using LoginMicroservice.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;

namespace LoginMicroservice.Api.Tests.Services;

public class LoginServicesTest
{

    private readonly ILoginServices _loginServices;
    private readonly IUserRepository _userRepository;
    private readonly User _userCorrect = new("a@a.com", "123456", false);

    public LoginServicesTest()
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

        _userRepository = new UserRepository(new LoginContext(new DbContextOptionsBuilder<LoginContext>().UseInMemoryDatabase(databaseName: "UserTestDb").Options));
        _loginServices = new LoginServices(_userRepository, new LoginDtoValidator(), twoFactorRepository, configuration.Object);
    }

    [Fact]
    public async Task ItShouldLogin()
    {
        await _userRepository.CreateAsync(new User("a@a.com", "123456", false));
        var result = await _loginServices.Call(new LoginDto("a@a.com", "123456"));
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task ItShouldTwoFactorLogin()
    {
        await _userRepository.CreateAsync(new User("a@a.com", "123456", true));
        var result = await _loginServices.Call(new LoginDto("a@a.com", "123456"));
        Assert.Null(result.Token);
        Assert.True(result.IsTwoFactorLogin);
    }

    [Fact]
    public async Task ItShouldNotFoundUser()
    {
        await Assert.ThrowsAsync<LoginContextException>(async () => await _loginServices.Call(new LoginDto("123@a.com", "1234588888")));
    }

    [Fact]
    public async Task ItShouldNotLogin()
    {
        await _userRepository.CreateAsync(new User("a@a.com", "123456", false));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _loginServices.Call(new LoginDto("a@a.com", "1234588888")));
    }

    [Fact]
    public async Task ItShouldThrowsValidationException()
    {
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _loginServices.Call(new LoginDto("aa.com", "13456")));
    }

    [Fact]
    public async Task ItShouldThrowsUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _loginServices.Call());
    }
}
