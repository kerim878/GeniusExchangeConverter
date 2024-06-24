using GeniusExchangeConverter.Domain.Entities;
using GeniusExchangeConverter.Infrastructure.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeniusExchangeConverter.Infrastructure.Data.Repositories;

public class ExchangeConversionRepository : IExchangeConversionRepository
{
    private readonly GeniusExchangeDbContext _dbContext;

    public ExchangeConversionRepository(GeniusExchangeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveConversionLog(ConversionLog log, CancellationToken cancellationToken = default)
    {
        _dbContext.ConversionLogs.Add(log);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConversionLog>> GetConversionLogs(
        string fromCurrency,
        string toCurrency,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ConversionLogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(fromCurrency))
            query = query.Where(x => x.FromCurrency == fromCurrency);

        if (!string.IsNullOrWhiteSpace(toCurrency))
            query = query.Where(x => x.ToCurrency == toCurrency);

        if (startDate.HasValue)
            query = query.Where(x => x.ConversionDate >= startDate);

        if (endDate.HasValue)
            query = query.Where(x => x.ConversionDate <= endDate);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}