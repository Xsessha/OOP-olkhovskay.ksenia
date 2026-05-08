namespace IndependentWork21.Strategy;

public class DataContext
{
    private IDataProcessorStrategy _strategy;

    public DataContext(IDataProcessorStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void SetStrategy(IDataProcessorStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public string ExecuteProcessing(string data)
    {
        return _strategy.Process(data);
    }
}
