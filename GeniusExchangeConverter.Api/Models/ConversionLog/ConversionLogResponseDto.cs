namespace GeniusExchangeConverter.Api.Models.ConversionLog;

public record ConversionLogResponseDto(
    int Id,
    string FromCurrency,
    string ToCurrency,
    decimal Amount,
    decimal ConvertedAmount,
    DateTime ConversionDate);