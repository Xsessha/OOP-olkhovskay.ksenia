using IndependentWork21.Factory;
using IndependentWork21.Observer;
using IndependentWork21.Services;
using IndependentWork21.Singleton;
using IndependentWork21.Strategy;

namespace IndependentWork21.Tests;


public class IntegrationTests
{
    [Fact]
    public void Positive_FactoryStrategyServiceSingletonObserver_CelsiusScenario()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        var databaseObserver = new WeatherDatabaseObserver();
        consoleObserver.Subscribe(publisher);
        databaseObserver.Subscribe(publisher);

        var service = new ProcessingService(
            new DataContext(StrategyFactory.Create("celsius")),
            publisher);

        var result = service.Process("25");

        Assert.Equal("Celsius -> Fahrenheit: 77 F", result);
        Assert.Equal(result, AppState.Instance.LastProcessedData);
        Assert.Equal(result, consoleObserver.ReceivedData.Single());
        Assert.Equal(result, databaseObserver.SavedData.Single());
    }

    [Fact]
    public void Positive_RuntimeStrategyChange_UpdatesStateAndNotifiesObserversInOrder()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        var databaseObserver = new WeatherDatabaseObserver();
        consoleObserver.Subscribe(publisher);
        databaseObserver.Subscribe(publisher);

        var service = new ProcessingService(
            new DataContext(StrategyFactory.Create("fahrenheit")),
            publisher);

        var firstResult = service.Process("212");
        service.ChangeStrategy(StrategyFactory.Create("wind"));
        var secondResult = service.Process("10");

        Assert.Equal("Fahrenheit -> Celsius: 100 C", firstResult);
        Assert.Equal("Wind speed: 36 km/h", secondResult);
        Assert.Equal(secondResult, AppState.Instance.LastProcessedData);
        Assert.Equal(new[] { firstResult, secondResult }, consoleObserver.ReceivedData);
        Assert.Equal(new[] { firstResult, secondResult }, databaseObserver.SavedData);
    }

    [Fact]
    public void Positive_FactoryAcceptsTrimmedCaseInsensitiveType_InFullScenario()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var publisher = new DataPublisher();
        var notifications = new List<string>();
        publisher.DataProcessed += notifications.Add;

        var strategy = StrategyFactory.Create("  CELSIUS  ");
        var service = new ProcessingService(new DataContext(strategy), publisher);

        var result = service.Process("-40");

        Assert.IsType<CelsiusToFahrenheitStrategy>(strategy);
        Assert.Equal("Celsius -> Fahrenheit: -40 F", result);
        Assert.Equal(result, AppState.Instance.LastProcessedData);
        Assert.Equal(result, notifications.Single());
    }

    [Fact]
    public void Negative_UnknownStrategyType_DoesNotChangeExistingSingletonState()
    {
        AppState.Instance.LastProcessedData = "previous valid result";

        var exception = Assert.Throws<ArgumentException>(() => StrategyFactory.Create("pressure"));

        Assert.Contains("Unknown strategy type", exception.Message);
        Assert.Equal("previous valid result", AppState.Instance.LastProcessedData);
    }

    [Fact]
    public void Negative_InvalidInput_IsPublishedAsErrorAndStoredInSingleton()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        consoleObserver.Subscribe(publisher);

        var service = new ProcessingService(
            new DataContext(StrategyFactory.Create("wind")),
            publisher);

        var result = service.Process("not-a-number");

        Assert.Equal("Invalid wind data", result);
        Assert.Equal(result, AppState.Instance.LastProcessedData);
        Assert.Equal(result, consoleObserver.ReceivedData.Single());
    }

    [Fact]
    public void Boundary_ObserverCanUnsubscribe_FromIntegrationFlow()
    {
        AppState.Instance.LastProcessedData = string.Empty;
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        var databaseObserver = new WeatherDatabaseObserver();
        consoleObserver.Subscribe(publisher);
        databaseObserver.Subscribe(publisher);

        var service = new ProcessingService(
            new DataContext(StrategyFactory.Create("celsius")),
            publisher);

        var firstResult = service.Process("0");
        databaseObserver.Unsubscribe(publisher);
        var secondResult = service.Process("100");

        Assert.Equal(new[] { firstResult, secondResult }, consoleObserver.ReceivedData);
        Assert.Equal(firstResult, databaseObserver.SavedData.Single());
        Assert.Equal(secondResult, AppState.Instance.LastProcessedData);
    }
}
