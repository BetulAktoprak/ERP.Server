using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ERP.Server.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrasturcture(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(action => action
        .FromAssemblies(typeof(DependencyInjection).Assembly)
        .AddClasses(publicOnly: false)
        .UsingRegistrationStrategy(registrationStrategy: RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );

        return services;
    }
}
