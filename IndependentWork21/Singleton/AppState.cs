namespace IndependentWork21.Singleton;

public class AppState
{
    private static AppState? _instance;

    public string LastProcessedData { get; set; } = "";

    private AppState()
    {
    }

    public static AppState Instance
    {
        get
        {
            _instance ??= new AppState();
            return _instance;
        }
    }
}