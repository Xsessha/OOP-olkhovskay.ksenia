using System.Collections.Generic;

namespace IndependentWork21.Observer;

public class ConsoleOutputObserver
{
    private readonly List<string> _receivedData = new();

    public IReadOnlyList<string> ReceivedData => _receivedData;

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
        _receivedData.Add(data);
        Console.WriteLine($"[Console] Received: {data}");
    }
}
