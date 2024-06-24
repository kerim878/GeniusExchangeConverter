using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GeniusExchangeConverter.Infrastructure.Configuration.Models;

public class OpenExchangeRatesOptions
{
    public string AppId { get; set; }
    public string BaseUrl { get; set; }
}

public class OpenExchangeRatesOptionsSetup : IConfigureOptions<OpenExchangeRatesOptions>
{
    private readonly IConfiguration _configuration;
    private const string SectionName = "OpenExchangeRates";

    public OpenExchangeRatesOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(OpenExchangeRatesOptions options)
    {
        _configuration.GetSection(SectionName)
            .Bind(options);
    }
}