using GeniusExchangeConverter.Domain.Entities;
using GeniusExchangeConverter.Domain.Models.CurrencyConversion;
using GeniusExchangeConverter.Infrastructure.Data.Repositories;
using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;

namespace GeniusExchangeConverter.Application.CurrencyService;

public sealed class CurrencyService : ICurrencyService
{
    private readonly IExchangeConversionRepository _exchangeConversionRepository;
    private readonly IOpenExchangeRateService _openExchangeRateService;

    public CurrencyService(IOpenExchangeRateService openExchangeRateService,
        IExchangeConversionRepository exchangeConversionRepository)
    {
        _openExchangeRateService = openExchangeRateService;
        _exchangeConversionRepository = exchangeConversionRepository;
    }

    public async Task<CurrencyConversionResponse> ConvertCurrency(CurrencyConversionRequest request,
        CancellationToken cancellationToken = default)
    {
        var rates = await _openExchangeRateService.GetLatestRates(cancellationToken);
        if (!rates.Rates.TryGetValue(request.FromCurrency, out var fromRate) ||
            !rates.Rates.TryGetValue(request.ToCurrency, out var toRate))
            throw new ArgumentException("Invalid currency code");

        var rate = toRate / fromRate;
        var convertedAmount = request.Amount * rate;

        var response = new CurrencyConversionResponse(
            request.FromCurrency,
            request.ToCurrency,
            request.Amount,
            convertedAmount,
            rate);

        await SaveConversionLog(response, cancellationToken);

        return response;
    }

    public async Task<IReadOnlyCollection<ConversionLog>> GetConversionLogs(
        string fromCurrency,
        string toCurrency,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        return await _exchangeConversionRepository.GetConversionLogs(fromCurrency, toCurrency, startDate, endDate,
            cancellationToken);
    }

    private async Task SaveConversionLog(CurrencyConversionResponse conversionResponse,
        CancellationToken cancellationToken)
    {
        var log = new ConversionLog
        {
            FromCurrency = conversionResponse.FromCurrency,
            ToCurrency = conversionResponse.ToCurrency,
            Amount = conversionResponse.OriginalAmount,
            ConvertedAmount = conversionResponse.ConvertedAmount,
            ConversionDate = DateTime.UtcNow
        };

        await _exchangeConversionRepository.SaveConversionLog(log, cancellationToken);
    }
}