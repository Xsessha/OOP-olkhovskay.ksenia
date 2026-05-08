using IndependentWork21.Factory;
using IndependentWork21.Observer;
using IndependentWork21.Services;
using IndependentWork21.Singleton;
using IndependentWork21.Strategy;

namespace IndependentWork21.Tests;

public class SingletonTests
{
    [Fact]
    public void Singleton_ShouldReturnSameInstance()
    {
        var first = AppState.Instance;
        var second = AppState.Instance;

        Assert.Same(first, second);
    }

    [Fact]
    public void Singleton_ShouldKeepStateBetweenAccesses()
    {
        AppState.Instance.LastProcessedData = "Weather";

        var secondAccess = AppState.Instance;

        Assert.Equal("Weather", secondAccess.LastProcessedData);
    }

    [Fact]
    public void ProcessingService_ShouldUpdateSingletonWithLastResult()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var service = new ProcessingService(
            new DataContext(StrategyFactory.Create("celsius")),
            new DataPublisher());

        service.Process("100");
        service.ChangeStrategy(StrategyFactory.Create("fahrenheit"));
        var lastResult = service.Process("32");

        Assert.Equal("Fahrenheit -> Celsius: 0 C", lastResult);
        Assert.Equal(lastResult, AppState.Instance.LastProcessedData);
    }
}
