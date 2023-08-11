using LoginMicroservice.Domain.RedisRepositories;
using StackExchange.Redis;

namespace LoginMicroservice.Infra.RedisRepositories;

public class TwoFactorRepository : ITwoFactorRepository
{
    private readonly IDatabase _database;

    public TwoFactorRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<string?> GetAsync(string code)
    {
        var value = await _database.StringGetAsync(code);
        return value;
    }

    public async Task SetAsync(string code, string userId, TimeSpan? expiry = null)
    {
       await  _database.StringSetAsync(code, userId, expiry);
    }
}
