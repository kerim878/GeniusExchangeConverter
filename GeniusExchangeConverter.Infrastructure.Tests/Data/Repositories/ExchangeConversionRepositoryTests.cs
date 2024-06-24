using AutoFixture;
using AutoFixture.AutoMoq;
using GeniusExchangeConverter.Domain.Entities;
using GeniusExchangeConverter.Infrastructure.Data.Persistence;
using GeniusExchangeConverter.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GeniusExchangeConverter.Infrastructure.Tests.Data.Repositories
{
    public class ExchangeConversionRepositoryTests
    {
        private IFixture _fixture;
        private GeniusExchangeDbContext _dbContext;
        private ExchangeConversionRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            var options = new DbContextOptionsBuilder<GeniusExchangeDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new GeniusExchangeDbContext(options);
            _repository = new ExchangeConversionRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task SaveConversionLog_ShouldAddLogToDatabase()
        {
            // Arrange
            var log = _fixture.Create<ConversionLog>();

            // Act
            await _repository.SaveConversionLog(log);

            // Assert
            var logs = await _dbContext.ConversionLogs.ToListAsync();
            Assert.That(1, Is.EqualTo(logs.Count));
            Assert.That(log.FromCurrency, Is.EqualTo(logs[0].FromCurrency));
            Assert.That(log.ToCurrency, Is.EqualTo(logs[0].ToCurrency));
            Assert.That(log.Amount, Is.EqualTo(logs[0].Amount));
            Assert.That(log.ConvertedAmount, Is.EqualTo(logs[0].ConvertedAmount));
        }

        [Test]
        public async Task GetConversionLogs_ShouldReturnLogsFilteredByFromCurrency()
        {
            // Arrange
            var logs = _fixture.CreateMany<ConversionLog>(5).ToList();
            logs[0].FromCurrency = "USD";
            logs[1].FromCurrency = "USD";
            logs[2].FromCurrency = "EUR";
            logs[3].FromCurrency = "USD";
            logs[4].FromCurrency = "GBP";
            await _dbContext.ConversionLogs.AddRangeAsync(logs);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversionLogs("USD", null, null, null);

            // Assert
            Assert.That(3, Is.EqualTo(result.Count));
            Assert.That(result.All(x => x.FromCurrency == "USD"), Is.True);
        }

        [Test]
        public async Task GetConversionLogs_ShouldReturnLogsFilteredByToCurrency()
        {
            // Arrange
            var logs = _fixture.CreateMany<ConversionLog>(5).ToList();
            logs[0].ToCurrency = "EUR";
            logs[1].ToCurrency = "USD";
            logs[2].ToCurrency = "EUR";
            logs[3].ToCurrency = "GBP";
            logs[4].ToCurrency = "EUR";
            await _dbContext.ConversionLogs.AddRangeAsync(logs);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversionLogs(null, "EUR", null, null);

            // Assert
            Assert.That(3, Is.EqualTo(result.Count));
            Assert.That(result.All(x => x.ToCurrency == "EUR"), Is.True);
        }

        [Test]
        public async Task GetConversionLogs_ShouldReturnLogsFilteredByDateRange()
        {
            // Arrange
            var logs = _fixture.CreateMany<ConversionLog>(5).ToList();
            logs[0].ConversionDate = DateTime.UtcNow.AddDays(-10);
            logs[1].ConversionDate = DateTime.UtcNow.AddDays(-5);
            logs[2].ConversionDate = DateTime.UtcNow.AddDays(-1);
            logs[3].ConversionDate = DateTime.UtcNow.AddDays(1);
            logs[4].ConversionDate = DateTime.UtcNow.AddDays(5);
            await _dbContext.ConversionLogs.AddRangeAsync(logs);
            await _dbContext.SaveChangesAsync();

            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow.AddDays(3);

            // Act
            var result = await _repository.GetConversionLogs(null, null, startDate, endDate);

            // Assert
            Assert.That(3, Is.EqualTo(result.Count));
            Assert.That(result.All(x => x.ConversionDate >= startDate && x.ConversionDate <= endDate), Is.True);
        }
    }
}
