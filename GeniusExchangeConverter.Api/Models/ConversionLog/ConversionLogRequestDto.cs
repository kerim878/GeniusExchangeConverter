namespace GeniusExchangeConverter.Api.Models.ConversionLog;

public record ConversionLogRequestDto(
    string FromCurrency,
    string ToCurrency,
    DateTime? StartDate,
    DateTime? EndDate);