using LoginMicroservice.Domain.Repositories;
using LoginMicroservice.Infra.Data;
using LoginMicroservice.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoginMicroservice.Infra.Injections;

public static class LoginContextInjector
{
    public static IServiceCollection AddContextAndRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LoginContext>(options => options.UseSqlServer(connectionString),ServiceLifetime.Singleton);
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}