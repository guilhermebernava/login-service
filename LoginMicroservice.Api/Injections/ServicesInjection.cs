using LoginMicroservice.Api.Services;
using LoginMicroservice.Api.Services.Interfaces;
namespace LoginMicroservice.Api.Injections;

public static class ServicesInjection
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserServices, CreateUserServices>();
        services.AddScoped<ILoginServices, LoginServices>();
        services.AddScoped<ITwoFactorLoginServices, TwoFactorLoginServices>();
    }
}
