namespace IndependentWork24.Composite;

public class DirectoryComposite : IFileSystemComponent
{
    private readonly List<IFileSystemComponent> _children = new();

    public string Name { get; }

    public long Size => _children.Sum(c => c.Size);

    public DirectoryComposite(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я директорії не може бути порожнім.", nameof(name));
        Name = name;
    }

    public void Add(IFileSystemComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);
        _children.Add(component);
    }

    public bool Remove(IFileSystemComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);
        return _children.Remove(component);
    }

    public IReadOnlyList<IFileSystemComponent> GetChildren() => _children.AsReadOnly();

    public int ChildCount => _children.Count;

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)} {Name}/ ({Size} bytes)");
        foreach (var child in _children)
            child.Display(depth + 1);
    }
}