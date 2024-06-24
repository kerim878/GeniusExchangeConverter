using FluentValidation;

namespace GeniusExchangeConverter.Api.Models.CurrencyConversion;

public record CurrencyConversionRequestDto(
    string FromCurrency,
    string ToCurrency,
    decimal Amount);

public class CurrencyConversionRequestValidator : AbstractValidator<CurrencyConversionRequestDto>
{
    public CurrencyConversionRequestValidator()
    {
        RuleFor(x => x.FromCurrency).NotEmpty().Length(3);
        RuleFor(x => x.ToCurrency).NotEmpty().Length(3);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}