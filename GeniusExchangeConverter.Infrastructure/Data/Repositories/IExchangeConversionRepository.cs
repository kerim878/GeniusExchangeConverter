using GeniusExchangeConverter.Domain.Entities;

namespace GeniusExchangeConverter.Infrastructure.Data.Repositories;

public interface IExchangeConversionRepository
{
    Task SaveConversionLog(ConversionLog log, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConversionLog>> GetConversionLogs(
        string fromCurrency,
        string toCurrency,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default);
}