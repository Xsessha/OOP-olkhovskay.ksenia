using IndependentWork24.Composite;

namespace IndependentWork24.Decorator;

public class LoggingDecorator : FileDecoratorBase
{
    private readonly List<string> _log = new();

    public LoggingDecorator(IFileSystemComponent component) : base(component) { }

    public override string Name => Wrapped.Name;

    public override long Size
    {
        get
        {
            long size = Wrapped.Size;
            _log.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] GetSize('{Wrapped.Name}') => {size} bytes");
            return size;
        }
    }

    public override void Display(int depth = 0)
    {
        _log.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Display('{Wrapped.Name}', depth={depth})");
        Wrapped.Display(depth);
    }

    public IReadOnlyList<string> GetLog() => _log.AsReadOnly();

    public void ClearLog() => _log.Clear();
}