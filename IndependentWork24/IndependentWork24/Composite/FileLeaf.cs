namespace IndependentWork24.Composite;

public class FileLeaf : IFileSystemComponent
{
    private string _content;

    public string Name { get; }
    public long Size { get; }

    public FileLeaf(string name, long size, string content = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я файлу не може бути порожнім.", nameof(name));
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size), "Розмір файлу не може бути від'ємним.");

        Name = name;
        Size = size;
        _content = content;
    }

    public string GetContent() => _content;

    public void SetContent(string content) => _content = content;

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)} {Name} ({Size} bytes)");
    }
}