using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate.Models;
using Refit;

namespace GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;

public interface IOpenExchangeRateClient
{
    [Get("/latest.json")]
    Task<OpenExchangeRateResponse> GetLatestRates([AliasAs("app_id")] string apiKey, CancellationToken cancellationToken = default);
}