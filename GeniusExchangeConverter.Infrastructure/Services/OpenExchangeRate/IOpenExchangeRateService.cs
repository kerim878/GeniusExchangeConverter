using GeniusExchangeConverter.Infrastructure.Configuration.Models;
using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate.Models;
using Microsoft.Extensions.Options;

namespace GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;

public interface IOpenExchangeRateService
{
    Task<OpenExchangeRateResponse> GetLatestRates(CancellationToken cancellationToken = default);
}

public class OpenExchangeRateService : IOpenExchangeRateService
{
    private readonly IOpenExchangeRateClient _exchangeRateClient;
    private readonly OpenExchangeRatesOptions _openExchangeRatesOptions;

    public OpenExchangeRateService(IOpenExchangeClientFactory exchangeRateClientFactory,
        IOptions<OpenExchangeRatesOptions> options)
    {
        _exchangeRateClient = exchangeRateClientFactory.CreateClient();
        _openExchangeRatesOptions = options.Value;
    }

    public Task<OpenExchangeRateResponse> GetLatestRates(CancellationToken cancellationToken = default)
    {
        return _exchangeRateClient.GetLatestRates(_openExchangeRatesOptions.AppId, cancellationToken);
    }
}