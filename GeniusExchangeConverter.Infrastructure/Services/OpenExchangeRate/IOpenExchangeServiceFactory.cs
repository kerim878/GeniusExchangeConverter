using GeniusExchangeConverter.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Options;
using Refit;

namespace GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;

public interface IOpenExchangeClientFactory
{
    IOpenExchangeRateClient CreateClient();
}

public class OpenExchangeClientFactory : IOpenExchangeClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenExchangeRatesOptions _openExchangeRatesOptions;

    public OpenExchangeClientFactory(IHttpClientFactory httpClientFactory, IOptions<OpenExchangeRatesOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _openExchangeRatesOptions = options.Value;
    }

    public IOpenExchangeRateClient CreateClient()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(_openExchangeRatesOptions.BaseUrl);
        return RestService.For<IOpenExchangeRateClient>(httpClient);
    }
}