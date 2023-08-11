using LoginMicroservice.Domain.RedisRepositories;
using LoginMicroservice.Infra.RedisRepositories;
using Moq;
using StackExchange.Redis;

namespace LoginMicroservice.Infra.Tests.RedisRepositories;

public class TwoFactorRepositoryTest
{
    private readonly ITwoFactorRepository twoFactorRepository;
    private readonly string _guid = "123";

    public TwoFactorRepositoryTest()
    {

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);
        var mockDatabase = new Mock<IDatabase>();
        mockMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);
        mockMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()).StringGetAsync(_guid, CommandFlags.None)).Returns(Task.FromResult<RedisValue>(_guid));

        twoFactorRepository = new TwoFactorRepository(mockMultiplexer.Object);
    }

    [Fact]
    public async Task ItShouldSetAnUser()
    {
        await twoFactorRepository.SetAsync(_guid, "12345");
    }

    [Fact]
    public async Task ItShouldGetAnUser()
    {
        var cachedUser = await twoFactorRepository.GetAsync(_guid);
        Assert.NotNull(cachedUser);
    }

    [Fact]
    public async Task ItShouldNotGetAnUser()
    {
        var cachedUser = await twoFactorRepository.GetAsync("");
        Assert.Null(cachedUser);
    }
}
