using GeniusExchangeConverter.Infrastructure.Configuration.Models;
using GeniusExchangeConverter.Infrastructure.Data.Persistence;
using GeniusExchangeConverter.Infrastructure.Data.Repositories;
using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GeniusExchangeConverter.Infrastructure.Configuration;

public static class InfrastructureConfiguration
{
    public static void ConfigureInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<OpenExchangeRatesOptionsSetup>();

        ConfigureDatabase(services, configuration);
        ConfigureServices(services);
    }

    public static void ApplyMigrations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<GeniusExchangeDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger>();
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GeniusExchangeDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("MasterConnection"));
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<IExchangeConversionRepository, ExchangeConversionRepository>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IOpenExchangeClientFactory, OpenExchangeClientFactory>();
        services.AddScoped<IOpenExchangeRateService, OpenExchangeRateService>();
    }
}