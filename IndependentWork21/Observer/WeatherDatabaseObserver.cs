using System.Collections.Generic;

namespace IndependentWork21.Observer;

public class WeatherDatabaseObserver
{
    private readonly List<string> _savedData = new();

    public IReadOnlyList<string> SavedData => _savedData;

    public void Subscribe(DataPublisher publisher)
    {
        publisher.DataProcessed += OnDataProcessed;
    }

    public void Unsubscribe(DataPublisher publisher)
    {
        publisher.DataProcessed -= OnDataProcessed;
    }

    private void OnDataProcessed(string data)
    {
        _savedData.Add(data);
        Console.WriteLine($"[Database] Saved: {data}");
    }
}
