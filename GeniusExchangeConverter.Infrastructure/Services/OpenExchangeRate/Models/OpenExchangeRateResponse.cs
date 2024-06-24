namespace GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate.Models;

public class OpenExchangeRateResponse
{
    public long Timestamp { get; set; }
    public string Base { get; set; }
    public Dictionary<string, decimal> Rates { get; set; } = new();
}