namespace GeniusExchangeConverter.Api.Models.CurrencyConversion;

public record CurrencyConversionResponseDto(
    string FromCurrency,
    string ToCurrency,
    decimal OriginalAmount,
    decimal ConvertedAmount,
    decimal ExchangeRate);