namespace GeniusExchangeConverter.Domain.Models.CurrencyConversion;

public record CurrencyConversionRequest(
    string FromCurrency,
    string ToCurrency,
    decimal Amount);