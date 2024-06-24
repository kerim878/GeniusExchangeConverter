namespace GeniusExchangeConverter.Domain.Entities;

public class ConversionLog
{
    public int Id { get; set; }
    public string FromCurrency { get; set; }
    public string ToCurrency { get; set; }
    public decimal Amount { get; set; }
    public decimal ConvertedAmount { get; set; }
    public DateTime ConversionDate { get; set; }
}