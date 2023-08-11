namespace LoginMicroservice.Domain.RedisRepositories;

public interface ITwoFactorRepository
{
    Task<string?> GetAsync(string code);
    Task SetAsync(string code, string userId, TimeSpan? expiry = null);
}
