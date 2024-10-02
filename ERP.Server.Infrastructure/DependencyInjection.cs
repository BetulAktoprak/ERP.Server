using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using ERP.Server.Infrastructure.Options;
using ERP.Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ERP.Server.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrasturcture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentEmail("info@erp.com")
            .AddSmtpSender("localhost", 25);

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>();

        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        //services.AddSignalR();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

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
