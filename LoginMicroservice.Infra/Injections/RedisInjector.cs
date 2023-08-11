using LoginMicroservice.Domain.RedisRepositories;
using LoginMicroservice.Infra.RedisRepositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LoginMicroservice.Infra.Injections;

public static class RedisInjector
{
    public static IServiceCollection AddRedisAndRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "Login-Service-Redis";
        });

        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(connectionString);
        });

        services.AddScoped<ITwoFactorRepository, TwoFactorRepository>();
        return services;
    }
}
