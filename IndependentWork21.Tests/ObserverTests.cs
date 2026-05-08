using IndependentWork21.Observer;

namespace IndependentWork21.Tests;

public class ObserverTests
{
    [Fact]
    public void ConsoleObserver_ShouldStoreReceivedNotifications()
    {
        var publisher = new DataPublisher();
        var observer = new ConsoleOutputObserver();
        observer.Subscribe(publisher);

        publisher.PublishDataProcessed("Data 1");
        publisher.PublishDataProcessed("Data 2");

        Assert.Equal(new[] { "Data 1", "Data 2" }, observer.ReceivedData);
    }

    [Fact]
    public void DatabaseObserver_ShouldStoreSavedNotifications()
    {
        var publisher = new DataPublisher();
        var observer = new WeatherDatabaseObserver();
        observer.Subscribe(publisher);

        publisher.PublishDataProcessed("Weather result");

        Assert.Equal("Weather result", observer.SavedData.Single());
    }

    [Fact]
    public void MultipleObservers_ShouldReceiveSameNotification()
    {
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        var databaseObserver = new WeatherDatabaseObserver();
        consoleObserver.Subscribe(publisher);
        databaseObserver.Subscribe(publisher);

        publisher.PublishDataProcessed("notification");

        Assert.Equal("notification", consoleObserver.ReceivedData.Single());
        Assert.Equal("notification", databaseObserver.SavedData.Single());
    }

    [Fact]
    public void UnsubscribedObserver_ShouldStopReceivingNotifications()
    {
        var publisher = new DataPublisher();
        var observer = new ConsoleOutputObserver();
        observer.Subscribe(publisher);

        publisher.PublishDataProcessed("first");
        observer.Unsubscribe(publisher);
        publisher.PublishDataProcessed("second");

        Assert.Equal("first", observer.ReceivedData.Single());
    }
}
