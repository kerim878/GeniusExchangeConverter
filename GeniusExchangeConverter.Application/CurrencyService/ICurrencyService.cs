using GeniusExchangeConverter.Domain.Entities;
using GeniusExchangeConverter.Domain.Models.CurrencyConversion;

namespace GeniusExchangeConverter.Application.CurrencyService;

public interface ICurrencyService
{
    Task<CurrencyConversionResponse> ConvertCurrency(CurrencyConversionRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConversionLog>> GetConversionLogs(
        string fromCurrency,
        string toCurrency,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default);
}