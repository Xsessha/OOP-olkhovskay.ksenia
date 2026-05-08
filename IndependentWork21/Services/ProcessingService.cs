using IndependentWork21.Strategy;
using IndependentWork21.Observer;
using IndependentWork21.Singleton;

namespace IndependentWork21.Services;

public class ProcessingService
{
    private readonly DataContext _context;
    private readonly DataPublisher _publisher;

    public ProcessingService(DataContext context, DataPublisher publisher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public string Process(string data)
    {
        var result = _context.ExecuteProcessing(data);

        Console.WriteLine(result);

        AppState.Instance.LastProcessedData = result;

        _publisher.PublishDataProcessed(result);

        return result;
    }

    public void ChangeStrategy(IDataProcessorStrategy strategy)
    {
        _context.SetStrategy(strategy);
    }
}
