namespace GeniusExchangeConverter.Domain.Models.CurrencyConversion;

public record CurrencyConversionResponse(
    string FromCurrency,
    string ToCurrency,
    decimal OriginalAmount,
    decimal ConvertedAmount,
    decimal ExchangeRate);