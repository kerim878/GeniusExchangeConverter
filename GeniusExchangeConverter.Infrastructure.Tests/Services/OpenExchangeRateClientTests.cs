using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate;
using GeniusExchangeConverter.Infrastructure.Services.OpenExchangeRate.Models;
using Moq;
using NUnit.Framework;
using Refit;

namespace GeniusExchangeConverter.Infrastructure.Tests.Services;

public class OpenExchangeRateClientTests
{
    private Mock<IOpenExchangeRateClient> _clientMock;
    private IFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _clientMock = _fixture.Freeze<Mock<IOpenExchangeRateClient>>();
    }

    [Test]
    public async Task GetLatestRates_ShouldReturnRates()
    {
        // Arrange
        var response = _fixture.Create<OpenExchangeRateResponse>();
        var apiKey = _fixture.Create<string>();
        _clientMock.Setup(client => client.GetLatestRates(apiKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var client = _clientMock.Object;

        // Act
        var result = await client.GetLatestRates(apiKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(response, Is.EqualTo(result));
        _clientMock.Verify(
            exchangeRateClient => exchangeRateClient.GetLatestRates(apiKey, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void GetLatestRates_ShouldThrowException_WhenApiKeyIsInvalid()
    {
        // Arrange
        var apiKey = _fixture.Create<string>();
        var apiException = ApiException.Create(new HttpRequestMessage(), HttpMethod.Get,
            new HttpResponseMessage(HttpStatusCode.Unauthorized), new RefitSettings()).Result;
        _clientMock.Setup(client => client.GetLatestRates(apiKey, It.IsAny<CancellationToken>()))
            .ThrowsAsync(apiException);

        var client = _clientMock.Object;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ApiException>(async () => await client.GetLatestRates(apiKey));
        Assert.That(HttpStatusCode.Unauthorized, Is.EqualTo(ex.StatusCode));
        _clientMock.Verify(
            exchangeRateClient => exchangeRateClient.GetLatestRates(apiKey, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void GetLatestRates_CancellationToken_ShouldCancelRequest()
    {
        // Arrange
        var apiKey = _fixture.Create<string>();
        var cancellationTokenSource = new CancellationTokenSource();
        _clientMock.Setup(client => client.GetLatestRates(apiKey, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException());

        var client = _clientMock.Object;

        // Act & Assert
        var ex = Assert.ThrowsAsync<TaskCanceledException>(async () => await client.GetLatestRates(apiKey, cancellationTokenSource.Token));
        Assert.That(ex, Is.InstanceOf<TaskCanceledException>());
        _clientMock.Verify(exchangeRateClient => exchangeRateClient.GetLatestRates(apiKey, It.IsAny<CancellationToken>()), Times.Once);
    }
}