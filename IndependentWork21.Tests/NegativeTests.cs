using IndependentWork21.Factory;
using IndependentWork21.Observer;
using IndependentWork21.Services;
using IndependentWork21.Strategy;

namespace IndependentWork21.Tests;

public class NegativeTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Factory_EmptyStrategyType_ShouldThrowArgumentException(string strategyType)
    {
        var exception = Assert.Throws<ArgumentException>(() => StrategyFactory.Create(strategyType));

        Assert.Contains("must be specified", exception.Message);
    }

    [Fact]
    public void Factory_NullStrategyType_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StrategyFactory.Create(null!));
    }

    [Theory]
    [InlineData("abc123")]
    [InlineData("--25")]
    [InlineData("25.5.5")]
    [InlineData("  ")]
    public void TemperatureStrategy_InvalidData_ShouldReturnError(string invalidData)
    {
        var strategy = StrategyFactory.Create("celsius");

        var result = strategy.Process(invalidData);

        Assert.Equal("Invalid temperature data", result);
    }

    [Fact]
    public void DataContext_NullStrategy_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => new DataContext(null!));
    }

    [Fact]
    public void ProcessingService_NullPublisher_ShouldThrow()
    {
        var context = new DataContext(StrategyFactory.Create("celsius"));

        Assert.Throws<ArgumentNullException>(() => new ProcessingService(context, null!));
    }

    [Fact]
    public void Publisher_WithoutSubscribers_ShouldNotThrow()
    {
        var publisher = new DataPublisher();

        var exception = Record.Exception(() => publisher.PublishDataProcessed("message"));

        Assert.Null(exception);
    }
}
