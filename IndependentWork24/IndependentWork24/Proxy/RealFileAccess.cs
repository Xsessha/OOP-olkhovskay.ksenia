namespace IndependentWork24.Proxy;

public class RealFileAccess : IFileAccess
{
    private string _content;

    public string FileName { get; }

    public bool CanRead => true;
    public bool CanWrite => true;

    public RealFileAccess(string fileName, string initialContent = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Ім'я файлу не може бути порожнім.", nameof(fileName));

        FileName = fileName;
        _content = initialContent;
    }

    public string Read() => _content;

    public void Write(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
        _content = content;
    }
}