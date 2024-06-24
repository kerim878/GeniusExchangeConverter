using GeniusExchangeConverter.Application.CurrencyService;
using Microsoft.Extensions.DependencyInjection;

namespace GeniusExchangeConverter.Application.Configuration;

public static class ApplicationConfiguration
{
    public static void ConfigureApplication(IServiceCollection services)
    {
        services.AddScoped<ICurrencyService, CurrencyService.CurrencyService>();
    }
}