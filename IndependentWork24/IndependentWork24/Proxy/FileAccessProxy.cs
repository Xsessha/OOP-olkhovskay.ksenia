namespace IndependentWork24.Proxy;

public class FileAccessProxy : IFileAccess
{
    private readonly RealFileAccess _real;
    private readonly UserRole _role;
    private readonly List<string> _accessLog = new();

    public bool CanRead => _role >= UserRole.Reader;

    public bool CanWrite => _role >= UserRole.Editor;

    public FileAccessProxy(RealFileAccess realAccess, UserRole role)
    {
        _real = realAccess ?? throw new ArgumentNullException(nameof(realAccess));
        _role = role;
    }

    public string Read()
    {
        if (!CanRead)
        {
            string msg = $"[DENIED] Читання заборонено для ролі '{_role}'.";
            _accessLog.Add(msg);
            throw new UnauthorizedAccessException(msg);
        }

        _accessLog.Add($"[OK] Читання дозволено для ролі '{_role}'.");
        return _real.Read();
    }

    public void Write(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (!CanWrite)
        {
            string msg = $"[DENIED] Запис заборонено для ролі '{_role}'.";
            _accessLog.Add(msg);
            throw new UnauthorizedAccessException(msg);
        }

        _accessLog.Add($"[OK] Запис дозволено для ролі '{_role}'.");
        _real.Write(content);
    }

    public IReadOnlyList<string> GetAccessLog() => _accessLog.AsReadOnly();

    public UserRole Role => _role;
}