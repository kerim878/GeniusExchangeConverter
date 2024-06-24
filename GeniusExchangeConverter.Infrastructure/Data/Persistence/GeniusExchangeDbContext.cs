using GeniusExchangeConverter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeniusExchangeConverter.Infrastructure.Data.Persistence;

public class GeniusExchangeDbContext : DbContext
{
    public GeniusExchangeDbContext(DbContextOptions<GeniusExchangeDbContext> options) : base(options)
    {
    }

    public DbSet<ConversionLog> ConversionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ConversionLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FromCurrency).IsRequired();
            entity.Property(e => e.ToCurrency).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.ConvertedAmount).IsRequired();
            entity.Property(e => e.ConversionDate).IsRequired();
        });
    }
}